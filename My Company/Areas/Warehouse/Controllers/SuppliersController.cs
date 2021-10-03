using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Repositories;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = Constants.Roles.MainAdministrator)]
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

        [HttpGet]
        public async Task<IActionResult> GetNIPs(string prefix)
        {
            if (prefix == null)
                return BadRequest();

            var nips = await _repositoryWrapper.SuppliersRepository.GetSuppliersNIPsByPrefix(prefix);
            List<object> list = new List<object>();
            foreach(string nip in nips)
            {
                list.Add(new { nip = nip });
            }

            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmails(string prefix)
        {
            if (prefix == null)
                return BadRequest();

            var emails = await _repositoryWrapper.SuppliersRepository.GetSuppliersEmailssByPrefix(prefix);
            List<object> list = new List<object>();
            foreach(string email in emails)
            {
                list.Add(new { email = email });
            }

            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetList(SuppliersListFilters filters)
        {
            if (filters == null)
                return BadRequest();

            return ViewComponent("SuppliersList", filters);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
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

            try
            {
                _repositoryWrapper.SuppliersRepository.Delete(supplier);
                await _repositoryWrapper.Save();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            

            return Ok();
        }

    }
}
