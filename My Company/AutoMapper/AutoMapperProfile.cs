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
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewWarehouseViewModel, Warehouse>();

            CreateMap<NewProductViewModel, Product>();

            CreateMap<CreateUserViewModel, AppUser>();

            CreateMap<EditEmployeeViewModel, AppUser>()
                .ReverseMap();

            CreateMap<CreateCategoryViewModel, Category>()
                .ForMember(
                x => x.ParentCategoryId,
                opt => opt.MapFrom(y => y.ParentCategoryId == -1 ? null : y.ParentCategoryId));

            CreateMap<CreteAttributeViewModel, Attribute>()
                .ReverseMap();

            CreateMap<WarehouseRow, WarehouseRowViewModel>();

            CreateMap<WarehouseSector, WarehouseSectorViewModel>();

            CreateMap<AppUser, EmployeeListItem>()
                .ForMember(x => x.NameAndSurname, opt => opt.MapFrom(y => $"{y.Name} {y.Surname}"))
                .ForMember(x => x.LockoutEnabled, opt => opt.MapFrom(y => y.LockoutEnd != null && y.LockoutEnd > DateTime.Now));

            CreateMap<Attribute, AttributeValuesViewModel>()
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Name))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Category, CategoryListItemViewModel>();

            CreateMap<Category, CategoryDetailsViewModel>();

            CreateMap<Attribute, AttributeDetailsViewModel>()
                .ForMember(a => a.Values, opt => opt.MapFrom(y => y.AttributeDictionaryValues.Select(aa => aa.Value)));

            CreateMap<string, AttributeDictionaryValues>()
                .ForMember(adv => adv.Value, opt => opt.MapFrom(y => y));

            CreateMap<Category, EditCategoryViewModel>()
                .ReverseMap();
        }
    }
}
