//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            if (await _repositoryWrapper.WarehouseRepository.FindAll().AnyAsync())
                return RedirectToAction(nameof(Details));
            return View();
        }

        public async Task<IActionResult> Details()
        {
            var warehouse = await _repositoryWrapper.WarehouseRepository.FindAll().FirstOrDefaultAsync();

            if (warehouse == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(warehouse);
        }

        public async Task<IActionResult> Create()
        {
            if (await _repositoryWrapper.WarehouseRepository.FindAll().AnyAsync())
                return RedirectToAction(nameof(Details));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewWarehouseViewModel warehouseDto)
        {
            if (await _repositoryWrapper.WarehouseRepository.FindAll().AnyAsync())
                return BadRequest();
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
                foreach (var sector in row.Sectors)
                {
                    sector.Deletable = await _repositoryWrapper.WarehouseSectorRepository.IsEmpty(sector.Id);
                }
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
            catch (Exception)
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

        [HttpDelete]
        public async Task<IActionResult> DeleteRow(int? RowId)
        {
            try
            {
                if (RowId == null)
                    return BadRequest();

                var row = await _repositoryWrapper.WarehouseRowRepository.GetById(RowId.Value);

                if (row == null)
                    return NotFound("there's no row with given id");

                foreach (var sector in row.Sectors)
                {
                    _repositoryWrapper.WarehouseSectorRepository.Delete(sector);
                }

                await _repositoryWrapper.WarehouseRowRepository.DeleteRow(row);

                await _repositoryWrapper.Save();

                return Ok(RowId);
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException || e is DbUpdateException)
                {
                    return BadRequest("cannot delete this row");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRow(NewWarehouseSectorViewModel newRow, int? WarehouseId)
        {
            try
            {
                if (newRow == null || WarehouseId == null)
                {
                    return BadRequest();
                }

                var warehouse = await _repositoryWrapper.WarehouseRepository.GetWithPlanById(WarehouseId.Value);

                if (warehouse == null)
                    return NotFound("warehouse not found");

                foreach (var row in warehouse.Rows)
                {
                    if (row.RowName == newRow.Name)
                    {
                        return BadRequest("name already exists");
                    }
                }

                var sectors = new List<WarehouseSector>();
                for (int i = 0; i < newRow.Count; i++)
                {
                    sectors.Add(new WarehouseSector()
                    {
                        Order = i + 1
                    });
                }

                WarehouseRow newRowDb = new()
                {
                    RowName = newRow.Name,
                    WarehouseId = warehouse.Id,
                    Order = warehouse.Rows.Count + 1,
                    Sectors = sectors
                };

                _repositoryWrapper.WarehouseRowRepository.Create(newRowDb);

                await _repositoryWrapper.Save();
                return Ok(newRowDb);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSector(int? SectorId)
        {
            try
            {
                if (SectorId == null)
                    return BadRequest();

                var sector = await _repositoryWrapper.WarehouseSectorRepository.GetByIdWithRow(SectorId.Value);

                if (sector == null)
                    return NotFound("invalid Id");

                if (!(await _repositoryWrapper.WarehouseSectorRepository.IsEmpty(SectorId.Value)))
                    return BadRequest("sector isn't empty");

                await _repositoryWrapper.WarehouseSectorRepository.DeleteSector(sector);
                await _repositoryWrapper.Save();

                var sectors = await _repositoryWrapper.WarehouseSectorRepository.GetSectorsByRow(sector.RowId);
                var sectorsDTOs = _mapper.Map<List<WarehouseSectorViewModel>>(sectors.OrderBy(s => s.Order));

                foreach (var s in sectorsDTOs)
                {
                    s.Deletable = await _repositoryWrapper.WarehouseSectorRepository.IsEmpty(s.Id);
                }

                return Ok(new { sectors = sectorsDTOs, rowId = sector.RowId, rowName = sector.Row.RowName });
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException || e is DbUpdateException)
                {
                    return BadRequest("cannot delete this row");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
