using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
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

        public ProductsController(IRepositoryWrapper repositoryWrapper,IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        //// GET: Warehouse/Products
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Supplier).Include(p => p.VATRate);
        //    return View(await applicationDbContext.ToListAsync());
        //}

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
            ViewData["CategoryId"] = new SelectList(repositoryWrapper.CategoriesRepository.FindAll(), "Id", "CategoryName");
            ViewData["SupplierId"] = ViewHelpers.GetSuppliersSelectList(repositoryWrapper.SuppliersRepository.FindAll(), null);
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(),null);
            return View();
        }

        // POST: Warehouse/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var productDb = mapper.Map<Product>(productViewModel);

                repositoryWrapper.ProductRepository.Create(productDb);

                await repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(repositoryWrapper.CategoriesRepository.FindAll(), "Id", "CategoryName", productViewModel.CategoryId);
            ViewData["SupplierId"] = ViewHelpers.GetSuppliersSelectList(repositoryWrapper.SuppliersRepository.FindAll(), productViewModel.SupplierId);
            ViewData["VATRateId"] = ViewHelpers.GetVatRatesSelectList(repositoryWrapper.VATRatesRepository.FindAll(), productViewModel.VATRateId);
            return View(productViewModel);
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
