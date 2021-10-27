using AutoMapper;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Dictionaries;
using My_Company.Extensions;
using My_Company.Models;
using System;
using System.Linq;
using Attribute = My_Company.Models.Attribute;

namespace My_Company.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewWarehouseViewModel, Warehouse>();

            CreateMap<CreateEditProductViewModel, Product>()
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(y => y.Categories))
                .ForMember(x => x.ProductAttributes, opt => opt.MapFrom(y => y.Attributes))
                .ForMember(x => x.NettoPrice, opt => opt.MapFrom(y => (int)(decimal.Parse(y.NettoPrice) * 100.0M)))
                .ForMember(x => x.Photos, opt => opt.Ignore());

            CreateMap<Photo, PhotoViewModel>();

            CreateMap<ProductCategory, int>()
                .ConstructUsing(p => p.CategoryId);

            CreateMap<Product, CreateEditProductViewModel>()
                .ForMember(x => x.Categories, opt => opt.MapFrom(y => y.ProductCategories))
                .ForMember(x => x.Attributes, opt => opt.MapFrom(y => y.ProductAttributes))
                .ForMember(x => x.NettoPrice, opt => opt.MapFrom(y => (decimal)(y.NettoPrice) / 100.0M));

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

            CreateMap<WarehouseSector, WarehouseSectorViewModel>()
                .ForMember(x => x.Barcode, opt => opt.MapFrom(y => y.Id.ToBarcode()));

            CreateMap<AppUser, EmployeeListItem>()
                .ForMember(x => x.NameAndSurname, opt => opt.MapFrom(y => $"{y.Name} {y.Surname}"))
                .ForMember(x => x.LockoutEnabled, opt => opt.MapFrom(y => y.LockoutEnd != null && y.LockoutEnd > DateTime.Now));

            CreateMap<Attribute, AttributeValuesViewModel>()
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Name))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Category, CategoryListItemViewModel>();

            CreateMap<Category, CategoryDetailsViewModel>();

            CreateMap<Attribute, AttributeViewModel>()
                .ForMember(a => a.Values, opt => opt.MapFrom(y => y.AttributeDictionaryValues.Select(aa => aa.Value)));

            CreateMap<string, AttributeDictionaryValues>()
                .ForMember(adv => adv.Value, opt => opt.MapFrom(y => y));

            CreateMap<AttributeDictionaryValues, string>()
                .ConvertUsing(a => a.Value);

            CreateMap<Category, EditCategoryViewModel>()
                .ReverseMap();

            CreateMap<Attribute, EditAttributeViewModel>()
                .ReverseMap();

            CreateMap<Supplier, SupplierViewModel>()
                .ReverseMap();

            CreateMap<Supplier, SupplierListItem>();

            CreateMap<Attribute, AttributeProductViewModel>()
                .ForMember(a => a.Values, opt => opt.MapFrom(a => a.AttributeDictionaryValues));

            CreateMap<AttributeProductViewModel, ProductAttribute>()
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(y => y.Value))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<int, ProductCategory>()
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(y => y));

            CreateMap<Product, ProductListItemViewModel>()
                .ForMember(x => x.SupplierData, opt => opt.MapFrom(y => y.Supplier.Name));

            CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(x => x.VATRate, opt => opt.MapFrom(y => y.VATRate.Rate + "%"))
                .ForMember(x => x.Supplier, opt => opt.MapFrom(y => y.Supplier.Name))
                .ForMember(x => x.Status, opt => opt.MapFrom(y => ProductStatusDictionary.ProductStatusesDictionary[y.Status]))
                .ForMember(x => x.NettoPrice, opt => opt.MapFrom(y => y.NettoPrice / 100.0M))
                .ForMember(x => x.Attributes, opt => opt.MapFrom(y => y.ProductAttributes));

            CreateMap<ProductAttribute, AttributeProductViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Attribute.Name))
                .ForMember(x => x.Type, opt => opt.MapFrom(y => y.Attribute.Type))
                .ForMember(x => x.Values, opt => opt.MapFrom(y => y.Attribute.AttributeDictionaryValues));

            CreateMap<AttributeDictionaryValues, string>()
                .ConvertUsing(x => x.Value);

            CreateMap<ProductBasicInfoEditViewModel, Product>()
                .ForMember(x => x.NettoPrice, opt => opt.MapFrom(y => (int)(decimal.Parse(y.NettoPrice) * 100.0M)));

            //deliveries
            CreateMap<DeliveryViewModel, Delivery>()
                .ForMember(x => x.DeliveryDate, opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.ProductDeliveries, opt => opt.MapFrom(y => y.Items));

            CreateMap<DeliveryItemViewModel, ProductDelivery>();
        }
    }
}
