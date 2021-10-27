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

        public DeliveriesController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        //// GET: Warehouse/Deliveries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = repositoryWrapper.DeliveriesRepository.FindAll().Include(d => d.Supplier);
            return View(await applicationDbContext.ToListAsync());
        }

        //// GET: Warehouse/Deliveries/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var delivery = await _context.Deliveries
        //        .Include(d => d.Supplier)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (delivery == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(delivery);
        //}


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
                    return BadRequest();

                var product = await repositoryWrapper.ProductRepository.GetProductByEANCode(ean);
                if (product == null)
                    return NotFound("invalid Code");
                if (product.SupplierId != supplierId)
                    return Ok(new { Error = "invalid supplier for this product" });
                if(product.Status == ProductStatus.Archived)
                    return Ok(new { Error = "product is archived" });

                var photo = product.Photos.FirstOrDefault(p => p.IsListPhoto);
                var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;

                return Ok(new { Product = new { Name=product.Name, Photo = photoUrl, Id=product.Id } });
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
                    return BadRequest();

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
            return Ok(sectors.Select(p => new { Id = p.Id, Order = p.Order, RowId = p.RowId}));
        }

        public async Task<IActionResult> ValidateSectorEan(string ean)
        {
            if (ean == null)
                return BadRequest(new { Error = "no ean given" });

            try
            {
                var id = int.Parse(ean);
                if (await repositoryWrapper.WarehouseSectorRepository.Exists(id))
                    return Ok(id);
                else
                    return Ok(new { Error = "sector not exists" });
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new { Error = "internal error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> New(DeliveryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Delivery deliveryDb = mapper.Map<Delivery>(model);
                    deliveryDb.ProductDeliveries = repositoryWrapper.DeliveriesRepository.RemoveDuplicates(deliveryDb.ProductDeliveries as List<ProductDelivery>);
                    repositoryWrapper.DeliveriesRepository.Create(deliveryDb);
                    await repositoryWrapper.Save();
                    return Ok(deliveryDb.Id);
                }

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //// GET: Warehouse/Deliveries/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var delivery = await _context.Deliveries.FindAsync(id);
        //    if (delivery == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "City", delivery.SupplierId);
        //    return View(delivery);
        //}

        //// POST: Warehouse/Deliveries/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierId,DeliveryDate")] Delivery delivery)
        //{
        //    if (id != delivery.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(delivery);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DeliveryExists(delivery.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "City", delivery.SupplierId);
        //    return View(delivery);
        //}

        //// GET: Warehouse/Deliveries/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var delivery = await _context.Deliveries
        //        .Include(d => d.Supplier)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (delivery == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(delivery);
        //}

        //// POST: Warehouse/Deliveries/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var delivery = await _context.Deliveries.FindAsync(id);
        //    _context.Deliveries.Remove(delivery);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DeliveryExists(int id)
        //{
        //    return _context.Deliveries.Any(e => e.Id == id);
        //}
    }
}
