using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Dictionaries;
using My_Company.Extensions;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = Constants.Roles.MainAdministrator)]
    public class ProductsController : Controller
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly IFilesService filesService;
        private readonly string rootPath;

        public ProductsController(IRepositoryWrapper repositoryWrapper, IMapper mapper, IFilesService filesService, IWebHostEnvironment environment)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.filesService = filesService;
            rootPath = environment.WebRootPath;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = new SelectList(await repositoryWrapper.CategoriesRepository.GetCategoriesTree(), "Id", "Tree");
            return View();
        }

        [HttpPost]
        public IActionResult GetList(ProductsListFilters filters)
        {
            if (filters == null)
            {
                return BadRequest();
            }

            return ViewComponent("ProductsList", filters);
        }

        [HttpGet]
        public async Task<IActionResult> GetEANs(string prefix)
        {
            if (prefix == null)
            {
                return BadRequest("no prefix given");
            }

            var codes = await repositoryWrapper.ProductRepository.GetCodesByPrefix(prefix);
            List<object> list = new();
            foreach (string code in codes)
            {
                list.Add(new { code = code });
            }

            return Ok(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await repositoryWrapper.ProductRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            var productViewModel = mapper.Map<ProductDetailsViewModel>(product);
            productViewModel.Photos = product.Photos.Where(p => !p.IsListPhoto).OrderByDescending(p => p.IsMainPhoto).Select(p => p.Path).ToList();
            productViewModel.Category = await repositoryWrapper
                .CategoriesRepository
                .GetCategoryTreeWithCategoryName(product.ProductCategories.FirstOrDefault(c => c.IsProductCategory).Category);

            return View(productViewModel);
        }

        // GET: Warehouse/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(repositoryWrapper.CategoriesRepository.ChildCategoriesById(null), "Id", "CategoryName");
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(), null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateEditProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var productDb = mapper.Map<Product>(productViewModel);
                productDb.ProductCategories.OrderByDescending(p => p.CategoryId).First().IsProductCategory = true;

                IEnumerable<string> paths = await filesService.UploadFiles(Request.Form.Files);

                foreach (var path in paths)
                {
                    productDb.Photos.Add(new Photo
                    {
                        Path = path
                    });
                }

                if (Request.Form.Files.Count() != 0)
                {
                    productDb.Photos.FirstOrDefault().IsMainPhoto = true;
                    var resizedImage = PhotosHelpers.GetResizedImage(Request.Form.Files[0], height: 300);
                    string listPhotoPath = filesService.UploadFile(resizedImage);
                    productDb.Photos.Add(new Photo
                    {
                        Path = listPhotoPath,
                        IsListPhoto = true
                    });
                }
                repositoryWrapper.ProductRepository.Create(productDb);

                await repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(repositoryWrapper.CategoriesRepository.ChildCategoriesById(null), "Id", "CategoryName");
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(), productViewModel.VATRateId);
            return View(productViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEAN(CreateEditProductViewModel newProductDto)
        {
            bool exists = await repositoryWrapper.ProductRepository.CheckEANExists(newProductDto.EANCode, newProductDto.Id);

            if (!exists)
            {
                return Json(true);
            }
            else
            {
                return Json("Podany kod jest już zajęty");
            }
        }

        [HttpGet]
        public IActionResult GetAttributesViewComponent(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            else
            {
                return ViewComponent("ProductAttributes", new { id = id.Value });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersToSelect(string query)
        {
            if (query == null)
            {
                return BadRequest();
            }

            var suppliers = await repositoryWrapper.SuppliersRepository.GetSuppliersByQuery(query);
            List<object> list = new List<object>();
            foreach (var supplier in suppliers)
            {
                list.Add(new { Id = supplier.Id, Description = $"{supplier.Name} \n{supplier.Street} {supplier.PostalCode} {supplier.City}" });
            }

            return Ok(list);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await repositoryWrapper.ProductRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            product.Photos = product.Photos.Where(p => !p.IsListPhoto).OrderByDescending(p => p.IsMainPhoto).ToList();
            var viewModel = mapper.Map<CreateEditProductViewModel>(product);

            List<SelectList> selects = new();
            foreach (var pc in product.ProductCategories)
            {
                selects.Add(new SelectList(repositoryWrapper.CategoriesRepository.ChildCategoriesById(pc.Category.ParentCategoryId), "Id", "CategoryName"));
            }

            ViewData["CategoryId"] = selects;
            var supplier = product.Supplier;
            ViewData["SupplierDescription"] = $"{supplier.Name} \n{supplier.Street} {supplier.PostalCode} {supplier.City}";
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(), product.VATRateId);
            ViewData["Statuses"] = new SelectList(ProductStatusDictionary.ProductStatusesDictionary, "Key", "Value", product.Status);
            return View(viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> EditCategories(EditProductCategoriesViewModel editProductCategories)
        {
            try
            {
                if (editProductCategories == null)
                {
                    return BadRequest();
                }

                var product = await repositoryWrapper.ProductRepository.GetProductWithCategoriesAndAttributesByIdTracked(editProductCategories.Id);

                if (product == null)
                {
                    return NotFound();
                }

                foreach (var pc in product.ProductCategories)
                {
                    pc.IsProductCategory = false;
                }

                product.ProductCategories = product.ProductCategories.Where(x => editProductCategories.Categories.Contains(x.CategoryId)).ToList();
                List<ProductCategory> newCategories = editProductCategories.Categories
                    .Where(x => !product.ProductCategories.Any(y => y.CategoryId == x) && x != 0)
                    .Select(x => new ProductCategory { CategoryId = x }).ToList();

                (product.ProductCategories as List<ProductCategory>).AddRange(newCategories);
                product.ProductCategories.OrderByDescending(p => p.CategoryId).First().IsProductCategory = true;

                foreach (var pa in product.ProductAttributes)
                {
                    if (!editProductCategories.Categories.Contains(pa.Attribute.CategoryId))
                    {
                        repositoryWrapper.ProductAttributeRepository.Delete(pa);
                    }
                }

                foreach (var c in newCategories)
                {
                    var attributes = (await repositoryWrapper.CategoryAttributesRepository.GetAttributesByCategoryId(c.CategoryId))
                        .Select(a => new ProductAttribute { AttributeId = a.Id });

                    product.ProductAttributes.AddRange(attributes);
                }

                repositoryWrapper.ProductRepository.Update(product);
                await repositoryWrapper.Save();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditDescription(EditProductDescriptionViewModel editProductDescriptionViewModel)
        {
            if (editProductDescriptionViewModel == null)
            {
                return BadRequest();
            }

            var product = await repositoryWrapper.ProductRepository.GetProductWithoutVirtualPropertiesById(editProductDescriptionViewModel.Id);

            if (product == null)
            {
                return NotFound();
            }

            product.Description = editProductDescriptionViewModel.Description;
            repositoryWrapper.ProductRepository.Update(product);

            await repositoryWrapper.Save();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditBasicInfo(ProductBasicInfoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (viewModel == null)
            {
                return BadRequest();
            }

            try
            {
                var product = await repositoryWrapper.ProductRepository.GetProductWithoutVirtualPropertiesById(viewModel.Id);

                if (product == null)
                {
                    return NotFound();
                }

                product = mapper.Map(viewModel, product);
                repositoryWrapper.ProductRepository.Update(product);

                await repositoryWrapper.Save();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditAttributes([FromRoute] int? id, List<AttributeProductViewModel> attributes)
        {
            try
            {
                if (id == null || attributes == null)
                {
                    return BadRequest();
                }

                var attributesDb = await repositoryWrapper.ProductRepository.GetAttributesByProductId(id.Value);
                foreach (var attribute in attributes)
                {
                    var attributeDb = attributesDb.FirstOrDefault(a => a.Id == attribute.Id);
                    if (attributeDb == null)
                    {
                        return BadRequest();
                    }

                    attributeDb.Value = attribute.Value;
                    repositoryWrapper.ProductAttributeRepository.Update(attributeDb);
                }

                await repositoryWrapper.Save();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(ChangeProductStatusViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return BadRequest();
                }

                var product = await repositoryWrapper.ProductRepository.GetProductWithoutVirtualPropertiesById(viewModel.Id);

                if (product == null)
                {
                    return BadRequest();
                }

                product.Status = viewModel.Status;

                repositoryWrapper.ProductRepository.Update(product);
                await repositoryWrapper.Save();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult GetAttributesComponent([FromRoute] int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                return ViewComponent("ProductAttributes", new { id = id.Value, isEdit = true });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto([FromForm] int? id, IFormFile file)
        {
            try
            {
                if (id == null || file == null)
                {
                    return BadRequest();
                }

                var productPhotos = await repositoryWrapper.PhotosRepository.GetPhotosByProduct(id.Value);
                bool isFirstPhoto = productPhotos.Count() == 0;
                var path = await filesService.UploadFile(file);
                var photo = new Photo { Path = path, ProductId = id.Value, IsMainPhoto = isFirstPhoto };
                productPhotos.Add(photo);
                repositoryWrapper.PhotosRepository.Create(photo);
                if (isFirstPhoto)
                {
                    var resizedImage = PhotosHelpers.GetResizedImage(file, height: 300);
                    string listPhotoPath = filesService.UploadFile(resizedImage);
                    repositoryWrapper.PhotosRepository.Create(new Photo
                    {
                        Path = listPhotoPath,
                        IsListPhoto = true,
                        ProductId = id.Value
                    });
                }
                await repositoryWrapper.Save();
                var photosViewModels = mapper.Map<List<PhotoViewModel>>(productPhotos.OrderByDescending(p => p.IsMainPhoto).Where(p => !p.IsListPhoto));
                return ViewComponent("EditPhotos", new { photos = photosViewModels });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePhoto([FromRoute] int? id, string path)
        {
            try
            {
                if (id == null || path == null)
                    return BadRequest();

                var productPhotos = await repositoryWrapper.PhotosRepository.GetPhotosByProduct(id.Value);
                var photoToDelete = productPhotos.FirstOrDefault(p => p.Path == path);
                if (photoToDelete == null)
                    return BadRequest();

                try
                {
                    filesService.DeletePhoto(path);
                }
                catch
                {

                }
                productPhotos.Remove(photoToDelete);
                repositoryWrapper.PhotosRepository.Delete(photoToDelete);
                if (photoToDelete.IsMainPhoto)
                {
                    var listPhoto = productPhotos.FirstOrDefault(p => p.IsListPhoto);
                    if (listPhoto != null)
                    {
                        filesService.DeletePhoto(listPhoto.Path);
                        repositoryWrapper.PhotosRepository.Delete(listPhoto);
                    }
                    if (productPhotos.Count > 1)
                    {
                        var newMainPhoto = productPhotos.FirstOrDefault(p => !p.IsListPhoto);
                        newMainPhoto.IsMainPhoto = true;
                        var newListImage = PhotosHelpers.GetResizedImage(Path.Join(rootPath, newMainPhoto.Path), height: 300);
                        var newListPhotoPath = filesService.UploadFile(newListImage);
                        listPhoto.Path = newListPhotoPath;
                        repositoryWrapper.PhotosRepository.Create(listPhoto);
                    }
                }

                await repositoryWrapper.Save();
                var photosViewModels = mapper.Map<List<PhotoViewModel>>(productPhotos.OrderByDescending(p => p.IsMainPhoto).Where(p => !p.IsListPhoto));

                return ViewComponent("EditPhotos", new { photos = photosViewModels });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
