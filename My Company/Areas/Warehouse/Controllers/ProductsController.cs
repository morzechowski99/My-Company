using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = Constants.Roles.MainAdministrator)]
    public class ProductsController : Controller
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly IFilesService filesService;

        public ProductsController(IRepositoryWrapper repositoryWrapper, IMapper mapper, IFilesService filesService)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.filesService = filesService;
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
                return BadRequest();

            return ViewComponent("ProductsList", filters);
        }

        [HttpGet]
        public async Task<IActionResult> GetEANs(string prefix)
        {
            if (prefix == null)
                return BadRequest("no prefix given");

            var codes = await repositoryWrapper.ProductRepository.GetCodesByPrefix(prefix);
            List<object> list = new();
            foreach (string code in codes)
            {
                list.Add(new { code = code });
            }

            return Ok(list);
        }

        //// GET: Warehouse/Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Supplier)
        //        .Include(p => p.VATRate)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // GET: Warehouse/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(repositoryWrapper.CategoriesRepository.ChildCategoriesById(null), "Id", "CategoryName");
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(), null);
            return View();
        }

        // POST: Warehouse/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var productDb = mapper.Map<Product>(productViewModel);

                IEnumerable<string> paths = await filesService.UploadFiles(Request.Form.Files);

                foreach (var path in paths)
                {
                    productDb.Photos.Add(new Photo
                    {
                        Path = path
                    });
                }

                var resizedImage = PhotosHelpers.GetResizedImage(Request.Form.Files[0], height: 300);
                string mainPath = filesService.UploadFile(resizedImage);
                productDb.Photos.Add(new Photo
                {
                    Path = mainPath,
                    IsListPhoto = true
                });

                repositoryWrapper.ProductRepository.Create(productDb);

                await repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(repositoryWrapper.CategoriesRepository.ChildCategoriesById(null), "Id", "CategoryName");
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(), productViewModel.VATRateId);
            return View(productViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEAN(NewProductViewModel newProductDto)
        {
            bool exists = await repositoryWrapper.ProductRepository.CheckEANExists(newProductDto.EANCode);

            if (!exists)
                return Json(true);
            else
                return Json("Podany kod jest już zajęty");
        }

        [HttpGet]
        public IActionResult GetAttributesViewComponent(int? id)
        {
            if (id == null)
                return BadRequest();

            else
                return ViewComponent("ProductAttributes", new { id = id.Value });
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersToSelect(string query)
        {
            if (query == null)
                return BadRequest();

            var suppliers = await repositoryWrapper.SuppliersRepository.GetSuppliersByQuery(query);
            List<object> list = new List<object>();
            foreach (var supplier in suppliers)
            {
                list.Add(new { Id = supplier.Id, Description = $"{supplier.Name} \n{supplier.Street} {supplier.PostalCode} {supplier.City}" });
            }

            return Ok(list);
        }

        // GET: Warehouse/Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Id", product.SupplierId);
        //    ViewData["VATRateId"] = new SelectList(_context.Set<VATRate>(), "Id", "Id", product.VATRateId);
        //    return View(product);
        //}

        //// POST: Warehouse/Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MagazineCount,Demand,EANCode,Description,CategoryId,SupplierId,VATRateId")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
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
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
        //    ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Id", product.SupplierId);
        //    ViewData["VATRateId"] = new SelectList(_context.Set<VATRate>(), "Id", "Id", product.VATRateId);
        //    return View(product);
        //}

        //// GET: Warehouse/Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Supplier)
        //        .Include(p => p.VATRate)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Warehouse/Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
