using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Repositories;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    public class SuppliersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public SuppliersController(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _repositoryWrapper.SuppliersRepository.GetById(id.Value);
            if (supplier == null)
            {
                return NotFound();
            }
            var supplierDto = _mapper.Map<SupplierViewModel>(supplier);

            return View(supplierDto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel supplier)
        {
            if (ModelState.IsValid)
            {
                var supplierDb = _mapper.Map<Supplier>(supplier);
                _repositoryWrapper.SuppliersRepository.Create(supplierDb);
                await _repositoryWrapper.Save();
                
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _repositoryWrapper.SuppliersRepository.GetById(id.Value);
            if (supplier == null)
            {
                return NotFound();
            }
            var supplierDto = _mapper.Map<SupplierViewModel>(supplier);
            return View(supplierDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierViewModel supplierDto)
        {
            if (id != supplierDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var supplier = await _repositoryWrapper.SuppliersRepository.GetById(id);
                if (supplier == null)
                {
                    return NotFound();
                }
                supplier = _mapper.Map(supplierDto, supplier);
                _repositoryWrapper.SuppliersRepository.Update(supplier);
                await _repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(supplierDto);
        }

        //// GET: Warehouse/Suppliers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var supplier = await _context.Suppliers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (supplier == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(supplier);
        //}

        //// POST: Warehouse/Suppliers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var supplier = await _context.Suppliers.FindAsync(id);
        //    _context.Suppliers.Remove(supplier);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SupplierExists(int id)
        //{
        //    return _context.Suppliers.Any(e => e.Id == id);
        //}
    }
}
