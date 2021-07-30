using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    public class WarehousesController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public WarehousesController(IMapper mapper,IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        // GET: Warehouse/Warehouses
        public async Task<IActionResult> Index()
        {
            return View(await _repositoryWrapper.WarehouseRepository.GetAll());
        }

        // GET: Warehouse/Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _repositoryWrapper.WarehouseRepository.GetById(id.Value);
          
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewWarehouseViewModel warehouse)
        {
            if (ModelState.IsValid)
            {
                var warehouseDb = _mapper.Map<Models.Warehouse>(warehouse);
                _repositoryWrapper.WarehouseRepository.Create(warehouseDb);
                await _repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouse/Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _repositoryWrapper.WarehouseRepository.GetById(id.Value);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouse/Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Street,PostalCode,City,Voivodeship")] My_Company.Models.Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_repositoryWrapper.WarehouseRepository.Update(warehouse);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!WarehouseExists(warehouse.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        //// GET: Warehouse/Warehouses/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var warehouse = await _context.Warehouses
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (warehouse == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(warehouse);
        //}

        // POST: Warehouse/Warehouses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var warehouse = await _context.Warehouses.FindAsync(id);
        //    _context.Warehouses.Remove(warehouse);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool WarehouseExists(int id)
        //{
        //    return _context.Warehouses.Any(e => e.Id == id);
        //}
    }
}
