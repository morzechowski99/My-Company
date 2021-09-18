using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class InheritedAttributesTableViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public InheritedAttributesTableViewComponent(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int parentCategoryId)
        {
            var attributes = await _repoWrapper.CategoriesRepository.GetAllAttributesByCategoryId(parentCategoryId);
            var attributesDtos = _mapper.Map<List<CreteAttributeViewModel>>(attributes);
          
            return View("InheritedAttributesTable",attributesDtos);
        }
    }
}
