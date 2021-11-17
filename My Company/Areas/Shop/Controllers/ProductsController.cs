﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Products;
using My_Company.Interfaces;

namespace My_Company.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class ProductsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public ProductsController(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        public IActionResult Search(int? id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult GetList(ProductsListFilters filters)
        {
            if (filters == null)
                return BadRequest();

            return ViewComponent("Products", filters);
        }
    }
}
