using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class ProductAttributesViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ProductAttributesViewComponent(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, bool isEdit = false)
        {
            List<AttributeProductViewModel> attributesDtos;
            if (!isEdit)
            {
                var attributes = await _repositoryWrapper.CategoriesRepository.GetAllAttributesByCategoryIdWithDictionaryValues(id);
                attributesDtos = _mapper.Map<List<AttributeProductViewModel>>(attributes);
            }
            else
            {
                var attributes = await _repositoryWrapper.ProductRepository.GetAttributesByProductId(id);
                attributesDtos = _mapper.Map<List<AttributeProductViewModel>>(attributes);
            }

            return View("ProductAttributes",new ProductsAttributesViewModel() { Attributes = attributesDtos});

        }

     
    }
}
