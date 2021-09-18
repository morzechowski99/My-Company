using AutoMapper;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = My_Company.Models.Attribute;

namespace My_Company.AutoMapper
{
    public class FromDTOProfile: Profile
    {
        public FromDTOProfile()
        {
            CreateMap<NewWarehouseViewModel, Warehouse>();
            CreateMap<NewProductViewModel, Product>();
            CreateMap<CreateUserViewModel, AppUser>();
            CreateMap<EditEmployeeViewModel, AppUser>();
            CreateMap<CreateCategoryViewModel, Category>()
                .ForMember(
                x => x.ParentCategoryId,
                opt => opt.MapFrom(y => y.ParentCategoryId == -1 ? null : y.ParentCategoryId));
            CreateMap<CreteAttributeViewModel, Attribute>();
        }
    }
}
