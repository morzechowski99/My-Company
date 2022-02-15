//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.EnumTypes;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Services.DocumentGeneratorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Policy = Constants.AuthorizationPolicies.WarehousePolicy)]
    public class DeliveriesController : Controller
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly IDocumentGenerator documentGenerator;

        public DeliveriesController(IRepositoryWrapper repositoryWrapper, IMapper mapper, IDocumentGenerator documentGenerator)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.documentGenerator = documentGenerator;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = repositoryWrapper.DeliveriesRepository.FindAll().Include(d => d.Supplier);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult GetList(DeliveriesListFilters filters)
        {
            if (filters == null)
            {
                return BadRequest();
            }

            return ViewComponent("DeliveriesList", filters);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var delivery = await repositoryWrapper.DeliveriesRepository.GetDeliveryById(id.Value);

            if (delivery == null)
            {
                return NotFound();
            }

            if (!delivery.IsCorrecting)
            {
                var deliveryDto = mapper.Map<DeliveryDetailsViewModel>(delivery);
                List<DeliveryProductViewModel> products = new();
                foreach (var pd in delivery.ProductDeliveries)
                {
                    var photo = pd.Product.Photos.FirstOrDefault(p => p.IsListPhoto);
                    var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                    var productDto = mapper.Map<DeliveryProductViewModel>(pd);
                    productDto.Photo = photoUrl;
                    products.Add(productDto);
                }
                deliveryDto.Products = products;

                return View(deliveryDto);
            }
            else
            {
                var orginal = await repositoryWrapper.DeliveriesRepository.GetDeliveryCorrectedDeliveryById(delivery.Id);
                var deliveryDto = mapper.Map<CorrectedDeliveryViewModel>(delivery);
                List<DeliveryCorrectedProductViewModel> products = new();
                foreach (var pd in delivery.ProductDeliveries)
                {
                    var photo = pd.Product.Photos.FirstOrDefault(p => p.IsListPhoto);
                    var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                    var productDto = mapper.Map<DeliveryCorrectedProductViewModel>(pd);
                    productDto.AfterCorrection.Photo = photoUrl;
                    var orgPd = orginal.ProductDeliveries.FirstOrDefault(p => p.ProductId == pd.ProductId && pd.SectorId == p.SectorId);
                    productDto.Orginal = new DeliveryItemViewModel { SectorId = orgPd.SectorId, ProductId = orgPd.ProductId, Count = orgPd.Count };
                    products.Add(productDto);
                }
                deliveryDto.Products = products;
                deliveryDto.CorrectedId = orginal.Id;
                deliveryDto.CorrectedNumber = orginal.PZNumber;
                return View("CorrectedDetails", deliveryDto);
            }

        }

        public IActionResult New()
        {
            ViewData["rows"] = new SelectList(repositoryWrapper.WarehouseRowRepository.FindAll(), "Id", "RowName");
            return View();
        }

        public async Task<IActionResult> GetProductByEan([FromQuery] string ean, [FromQuery] int? supplierId)
        {
            try
            {
                if (ean == null || supplierId == null)
                {
                    return BadRequest();
                }

                var product = await repositoryWrapper.ProductRepository.GetProductByEANCode(ean);
                if (product == null)
                {
                    return NotFound("invalid Code");
                }

                if (product.SupplierId != supplierId)
                {
                    return Ok(new { Error = "invalid supplier for this product" });
                }

                if (product.Status == ProductStatus.Archived)
                {
                    return Ok(new { Error = "product is archived" });
                }

                var photo = product.Photos.FirstOrDefault(p => p.IsListPhoto);
                var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;

                return Ok(new { Product = new { Name = product.Name, Photo = photoUrl, Id = product.Id } });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing request");
            }
        }

        public async Task<IActionResult> GetProductsByQuery(string query)
        {
            try
            {
                if (query == null)
                {
                    return BadRequest();
                }

                var products = await repositoryWrapper.ProductRepository.SearchProductByQueryStringWithoutArchived(query);

                return Ok(products.Select(p => new { Ean = p.EANCode, Description = $"{p.Name} ({p.EANCode})" }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> GetSectors()
        {
            var sectors = await repositoryWrapper.WarehouseSectorRepository.GetAll();
            return Ok(sectors.Select(p => new { Id = p.Id, Order = p.Order, RowId = p.RowId }));
        }

        public async Task<IActionResult> ValidateSectorEan(string ean)
        {
            if (ean == null)
            {
                return BadRequest(new { Error = "no ean given" });
            }

            try
            {
                var id = int.Parse(ean);
                if (await repositoryWrapper.WarehouseSectorRepository.Exists(id))
                {
                    return Ok(id);
                }
                else
                {
                    return Ok(new { Error = "sector not exists" });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "internal error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> New(DeliveryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var tr = await repositoryWrapper.BeginTransaction();
                    Delivery deliveryDb = mapper.Map<Delivery>(model);
                    deliveryDb.ProductDeliveries = repositoryWrapper.DeliveriesRepository.RemoveDuplicates(deliveryDb.ProductDeliveries as List<ProductDelivery>);
                    deliveryDb.PZNumber = await repositoryWrapper.DeliveriesRepository.CreatePZNumber();
                    foreach (var item in deliveryDb.ProductDeliveries)
                    {
                        var product = await repositoryWrapper.ProductRepository
                            .GetProductWithoutVirtualPropertiesById(item.ProductId);
                        product.StockQuantity += item.Count;
                        repositoryWrapper.ProductRepository.Update(product);
                        var productSector = await repositoryWrapper.ProductSectorRepository.GetByProductAndSector(item.ProductId, item.SectorId);
                        if (productSector == null)
                        {
                            repositoryWrapper.ProductSectorRepository.Create(new ProductSector
                            {
                                Count = item.Count,
                                ProductId = item.ProductId,
                                SectorId = item.SectorId
                            });
                        }
                        else
                        {
                            productSector.Count += item.Count;
                            repositoryWrapper.ProductSectorRepository.Update(productSector);
                        }
                        await repositoryWrapper.Save();
                        repositoryWrapper.ClearTracked();
                    }
                    repositoryWrapper.DeliveriesRepository.Create(deliveryDb);
                    await repositoryWrapper.Save();
                    await tr.CommitAsync();
                    return Ok(deliveryDb.Id);
                }

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> Correct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await repositoryWrapper.DeliveriesRepository.GetDeliveryById(id.Value);
            if (delivery == null || delivery.CorrectingId.HasValue)
            {
                return NotFound();
            }

            var deliveryDto = mapper.Map<DeliveryEditViewModel>(delivery);
            List<DeliveryProductCorrectViewModel> products = new();
            foreach (var pd in delivery.ProductDeliveries)
            {
                var photo = pd.Product.Photos.FirstOrDefault(p => p.IsListPhoto);
                var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                var productDto = mapper.Map<DeliveryProductCorrectViewModel>(pd);
                productDto.Photo = photoUrl;
                products.Add(productDto);
            }
            deliveryDto.Products = products;
            return View(deliveryDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Correct(int id, DeliveryEditViewModel deliveryDto)
        {
            if (id != deliveryDto.Id)
            {
                return NotFound();
            }
            Delivery deliveryDb = await repositoryWrapper.DeliveriesRepository.GetDeliveryById(id);
            if (deliveryDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Delivery correcting = new Delivery { IsCorrecting = true, SupplierId = deliveryDb.SupplierId, DeliveryDate = DateTime.Now };
                for (int i = 0; i < deliveryDto.Products.Count; i++)
                {
                    var item = deliveryDto.Products[i];
                    var orginal = deliveryDb.ProductDeliveries.FirstOrDefault(pd => pd.Id == item.Id);
                    var productSector = await repositoryWrapper.ProductSectorRepository.GetByProductAndSector(orginal.ProductId, orginal.SectorId);
                    if (productSector.Count - (orginal.Count - item.Count) < 0)
                    {
                        ModelState.AddModelError($"Products[{i}].Count", "W tym sektorze jest za mało produktów, aby usunąć tyle sztuk");
                        deliveryDto = mapDelivery(deliveryDto, deliveryDb);
                        return View(deliveryDto);
                    }
                    correcting.ProductDeliveries.Add(new() { ProductId = orginal.ProductId, SectorId = orginal.SectorId, Count = item.Count });
                    productSector.Count += item.Count - orginal.Count;
                    repositoryWrapper.ProductSectorRepository.Update(productSector);
                }
                deliveryDb.Correcting = correcting;
                correcting.PZNumber = await repositoryWrapper.DeliveriesRepository.CreateKPZNumber();
                deliveryDb.ProductDeliveries = null;
                repositoryWrapper.DeliveriesRepository.Update(deliveryDb);
                repositoryWrapper.DeliveriesRepository.Create(correcting);
                await repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            deliveryDto = mapDelivery(deliveryDto, deliveryDb);
            return View(deliveryDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetPdf(int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var delivery = await repositoryWrapper.DeliveriesRepository.GetDeliveryToDocumentById(id.Value);
                if (delivery == null)
                    return BadRequest();

                if (!delivery.IsCorrecting)
                {
                    return File(await documentGenerator.GetDeliveryDocument(delivery), "application/pdf", $"Dostawa nr {delivery.PZNumber}.pdf");
                }
                else
                {
                    var orginal = await repositoryWrapper.DeliveriesRepository.GetOrginalDeliveryToDocumentCorrectingById(id.Value);
                    return File(await documentGenerator.GetDeliveryCorrecingDocument(delivery, orginal), "application/pdf", $"Dostawa nr {delivery.PZNumber}.pdf");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private DeliveryEditViewModel mapDelivery(DeliveryEditViewModel deliveryDto, Delivery deliveryDb)
        {
            deliveryDto = mapper.Map(deliveryDb, deliveryDto);
            List<DeliveryProductCorrectViewModel> products = new();
            foreach (var pd in deliveryDb.ProductDeliveries)
            {
                var photo = pd.Product.Photos.FirstOrDefault(p => p.IsListPhoto);
                var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                var productDto = mapper.Map<DeliveryProductCorrectViewModel>(pd);
                productDto.Count = deliveryDto.Products.FirstOrDefault(p => p.Id == pd.Id).Count;
                productDto.Photo = photoUrl;
                products.Add(productDto);
            }
            deliveryDto.Products = products;
            return deliveryDto;
        }
    }
}
