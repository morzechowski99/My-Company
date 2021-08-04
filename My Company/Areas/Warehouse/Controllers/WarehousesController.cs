using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
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

        public WarehousesController(IMapper mapper, IRepositoryWrapper repositoryWrapper)
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
        public async Task<IActionResult> Create(NewWarehouseViewModel warehouseDto)
        {
            if (ModelState.IsValid)
            {
                var warehouseDb = _mapper.Map<Models.Warehouse>(warehouseDto);
                List<WarehouseRow> rows = new();
                int order = 1;

                foreach (var row in warehouseDto.Sectors)
                {
                    var sectors = new List<WarehouseSector>();
                    for (int i = 0; i < row.Count; i++)
                    {
                        sectors.Add(new WarehouseSector()
                        {
                            Order = i + 1
                        });
                    }
                    rows.Add(new WarehouseRow()
                    {
                        RowName = row.Name,
                        Order = order,
                        Sectors = sectors
                    });

                    order++;
                }

                warehouseDb.Rows = rows;
                _repositoryWrapper.WarehouseRepository.Create(warehouseDb);
                await _repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouseDto);
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
        public async Task<IActionResult> Edit(int id, My_Company.Models.Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repositoryWrapper.WarehouseRepository.Update(warehouse);
                    await _repositoryWrapper.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repositoryWrapper.WarehouseRepository.Exists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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

        [HttpGet]
        public async Task<IActionResult> EditPlan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _repositoryWrapper.WarehouseRepository.GetWithPlanById(id.Value);

            if (warehouse == null)
            {
                return NotFound();
            }

            var plan = _mapper.Map<List<WarehouseRowViewModel>>(warehouse.Rows);

            plan.Sort((r1, r2) => r1.Order - r2.Order > 0 ? 1 : -1);

            foreach (var row in plan)
            {
                row.Sectors.Sort((r1, r2) => r1.Order - r2.Order > 0 ? 1 : -1);
            }

            ViewData["WarehouseName"] = warehouse.Name;
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> AddSectors(AddSectorsViewModel newSecotrs)
        {
            try
            {
                if (newSecotrs == null || !ModelState.IsValid)
                {
                    return BadRequest();
                }

                var row = await _repositoryWrapper.WarehouseRowRepository.GetById(newSecotrs.RowId);
                var sectorsCount = row.Sectors.Count;

                for (int i = 1; i <= newSecotrs.Count; i++)
                {
                    row.Sectors.Add(new WarehouseSector()
                    {
                        Order = sectorsCount + i
                    });
                }

                _repositoryWrapper.WarehouseRowRepository.Update(row);

                await _repositoryWrapper.Save();

                row.Sectors = row.Sectors.OrderBy(s => s.Order).ToList();

                return Ok(row);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> SwapRows(int rowId, int direction)
        {
            try
            {
                if (direction != 1 && direction != -1)
                    return BadRequest("invalid direction");

                var row = await _repositoryWrapper.WarehouseRowRepository.GetById(rowId);

                if (row == null)
                    return NotFound("invalid rowId");


                int order = row.Order + direction;

                var secondRow = await _repositoryWrapper.WarehouseRowRepository.GetByOrderAndWarehouse(order, row.WarehouseId);

                if (secondRow == null)
                    return NotFound();

                int temporder = row.Order;
                row.Order = secondRow.Order;
                secondRow.Order = temporder;

                _repositoryWrapper.WarehouseRowRepository.Update(row);
                _repositoryWrapper.WarehouseRowRepository.Update(secondRow);

                await _repositoryWrapper.Save();

                return Ok(secondRow.Id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
