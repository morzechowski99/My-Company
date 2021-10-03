using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class SuppliersListViewComponent : ViewComponent
    {
        public IRepositoryWrapper _repositoryWrapper { get; set; }
        public IMapper _mapper { get; set; }

        public SuppliersListViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(SuppliersListFilters filters)
        {
            var suppliers = _repositoryWrapper.SuppliersRepository.GetSuppliersByFilters(filters);

            var list = await PagedList<Supplier>
                .ToPagedList(suppliers, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 25);

            var listView = new List<SupplierListItem>(); 
            foreach(var supplier in list)
            {
                var supplierDto = _mapper.Map<SupplierListItem>(supplier);
                supplierDto.Deletable = await _repositoryWrapper.SuppliersRepository.CheckSupplierDeletable(supplier);
                listView.Add(supplierDto);
            }


            return View("SuppliersList", new PagedList<SupplierListItem>(listView, suppliers.Count(), list.CurrentPage, list.PageSize));
        }
    }
}
