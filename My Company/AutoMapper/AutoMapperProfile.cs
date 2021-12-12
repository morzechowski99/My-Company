using AutoMapper;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Login;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Areas.Shop.ViewModels.Products;
using My_Company.Areas.Shop.ViewModels.Profile;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Dictionaries;
using My_Company.Extensions;
using My_Company.Helpers;
using My_Company.Models;
using My_Company.Models.Configuration;
using My_Company.Models.DBViews;
using My_Company.Services.DocumentGeneratorService.Models;
using My_Company.ViewModels;
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

            CreateMap<Delivery, DeliveryListItem>()
                .ForMember(d => d.Supplier, opt => opt.MapFrom(y => y.Supplier.Name));

            CreateMap<Delivery, DeliveryDetailsViewModel>()
                .ForMember(d => d.Supplier, opt => opt.MapFrom(y => y.Supplier.Name))
                .ForMember(d => d.WasCorrected, opt => opt.MapFrom(y => y.CorrectingId.HasValue));

            CreateMap<ProductDelivery, DeliveryProductViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Product.Name))
                .ForMember(x => x.Sector, opt => opt.MapFrom(y => y.Sector.Row.RowName + y.Sector.Order));

            CreateMap<Delivery, DeliveryEditViewModel>()
                .ForMember(d => d.Supplier, opt => opt.MapFrom(y => y.Supplier.Name));

            CreateMap<ProductDelivery, DeliveryProductCorrectViewModel>()
               .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Product.Name))
               .ForMember(x => x.Sector, opt => opt.MapFrom(y => y.Sector.Row.RowName + y.Sector.Order));

            CreateMap<Delivery, CorrectedDeliveryViewModel>()
              .ForMember(d => d.Supplier, opt => opt.MapFrom(y => y.Supplier.Name))
              .ForMember(d => d.Products, opt => opt.Ignore());

            CreateMap<ProductDelivery, DeliveryCorrectedProductViewModel>()
                .ForMember(x => x.AfterCorrection, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Orginal, opt => opt.Ignore());

            //orders
            CreateMap<OrdersToComplete, OrderListItemViewModel>();
            CreateMap<Order, OrderListItemViewModel>();
            CreateMap<Order, OrderPackingViewModel>();
            CreateMap<ProductOrder, ProductListItemViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Product.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Product.Name))
                .ForMember(x => x.EANCode, opt => opt.MapFrom(y => y.Product.EANCode))
                .ForMember(x => x.PhotoUrl, opt => opt.MapFrom(y => y.Product.Photos.Count == 0 ? Constants.ImagePlaceholder : y.Product.Photos.FirstOrDefault().Path));
                

            CreateMap<Order, OrderPickingViewModel>()
                .ForMember(x => x.Items, opt => opt.MapFrom(y => y.ProductOrders))
                .ForMember(x => x.PickedItems, opt => opt.MapFrom(y => y.Picking.PickingItems));

            CreateMap<ProductOrder, OrderPickingItemViewModel>()
                .ForMember(x => x.ProductOrderId, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.ProductSectors, opt => opt.MapFrom(y => y.Product.ProductSectors
                .Where(ps => ps.Count > 0)
                .OrderBy(ps => ps.Sector.Row.Order)
                .ThenBy(ps => ps.Sector.Order)
                .Take(5)));

            CreateMap<ProductSector, OrderPickingProductSector>()
                .ForMember(x => x.SectorName, opt => opt.MapFrom(y => y.Sector.Row.RowName + y.Sector.Order));

            CreateMap<PickingItem, PickedItemViewModel>()
                .ForMember(x => x.Sector, opt => opt.MapFrom(y => y.Sector.Row.RowName + y.Sector.Order))
                .ForMember(x => x.EANCode, opt => opt.MapFrom(y => y.ProductOrder.Product.EANCode))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(y => y.ProductOrder.Product.Name));

            CreateMap<Order, AllOrdersListItemViewModel>()
                .ForMember(x => x.Status, opt => opt.MapFrom(y => OrderStatusesDictionary.Dictionary[y.Status]));

            CreateMap<Order, OrderDetailsViewModel>()
                .ForMember(x => x.Status, opt => opt.MapFrom(y => OrderStatusesDictionary.Dictionary[y.Status]))
                .ForMember(x => x.OrderStatus, opt => opt.MapFrom(y => y.Status))
                .ForMember(x => x.PaymentMethod, opt => opt.MapFrom(y => PaymentMethodDictionary.PaymentDictionary[y.PaymentMethod]))         
                .ForMember(x => x.DeliveryMethod, opt => opt.MapFrom(y => DeliveryTypesDictionary.Dictionary[y.DeliveryType]))         
                .ForMember(x => x.ParcelLockerInfo, opt => opt.MapFrom(y => y.Delivery is InPostDelivery ? (y.Delivery as InPostDelivery).PackLockerName : null))         
                .ForMember(x => x.PickingUser, opt => opt.MapFrom(y => $"{y.Picking.User.Name} {y.Picking.User.Surname} ({y.User.UserName})"))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address.ToString()));

            CreateMap<ProductOrder, ProductOrderDetailsViewModel>()
                .ForMember(x => x.ProductDescritpion, opt => opt.MapFrom(y => y.Product.ToString()));

            //shop
            CreateMap<Product, ListItemViewModel>()
                .ForMember(x => x.Price, opt => opt.MapFrom(y => Helpers.ProductsHelpers.GetGrossPrice(y.NettoPrice, y.VATRate.Rate)))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(y => y.ProductCategories.First().Category.CategoryName));
            

            CreateMap<Product, ProductDetailsPageViewModel>()
                .ForMember(x => x.State, opt => opt.MapFrom(y => Helpers.ViewHelpers.GetProductStockStatus(y)))
                .ForMember(x => x.Price, opt => opt.MapFrom(y => Helpers.ProductsHelpers.GetGrossPrice(y.NettoPrice, y.VATRate.Rate)))
                .ForMember(x => x.Attributes, opt => opt.MapFrom(y => y.ProductAttributes))
                .ForMember(x => x.Category, opt => opt.MapFrom(y => y.ProductCategories.First(pc => pc.IsProductCategory).Category.CategoryName))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(y => y.ProductCategories.First(pc => pc.IsProductCategory).CategoryId))
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(y => y.ProductCategories.OrderBy(pc => pc.CategoryId)));

            CreateMap<Photo, string>()
                .ConvertUsing(p => p == null ? null : p.Path);

            CreateMap<Category, CategoryNameAndId>();

            CreateMap<ProductCategory, CategoryNameAndId>()
                .IncludeMembers(s => s.Category);

            CreateMap<Product, CartItem>()
                .ForMember(x => x.Price, opt => opt.MapFrom(y => Helpers.ProductsHelpers.GetGrossPrice(y.NettoPrice, y.VATRate.Rate)))
                .ForMember(x => x.Photo, opt => opt.MapFrom(y => y.Photos.FirstOrDefault()));

            //new order
            CreateMap<NewOrderModel, Order>()
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.ShippingAddress));

            CreateMap<AddressModel, Address>()
                .ReverseMap();

            //login register
            CreateMap<LoginModel, LoginRegisterModel>();
            CreateMap<RegisterModel, LoginRegisterModel>();

            //profile
            CreateMap<AppUser, UserViewModel>();
            CreateMap<Order, UserOrderListItemViewModel>()
                .ForMember(x => x.Status, opt => opt.MapFrom(y => Dictionaries.OrderStatusesDictionary.Dictionary[y.Status]))
                .ForMember(x => x.Total, opt => opt.MapFrom(y => Helpers.OrderHelpers.GetOrderAmmount(y)));

            //orderderails
            CreateMap<Order, OrderDefailsViewModel>()
                .ForMember(x => x.Status, opt => opt.MapFrom(y => Dictionaries.OrderStatusesDictionary.Dictionary[y.Status]))
                .ForMember(x => x.Total, opt => opt.MapFrom(y => Helpers.OrderHelpers.GetOrderAmmount(y)))
                .ForMember(x => x.DeliveryTypeString, opt => opt.MapFrom(y => Dictionaries.DeliveryTypesDictionary.Dictionary[y.DeliveryType]))
                .ForMember(x => x.PaymentMethodString, opt => opt.MapFrom(y => Dictionaries.PaymentMethodDictionary.PaymentDictionary[y.PaymentMethod]))
                .ForMember(x => x.PaymentPrice, opt => opt.MapFrom(y => Math.Round(y.PaymentPrice / 100.0M, 2)))
                .ForMember(x => x.DeliveryPrice, opt => opt.MapFrom(y => Math.Round(y.DeliveryPrice / 100.0M, 2)))
                .ForMember(x => x.Products, opt => opt.MapFrom(y =>y.ProductOrders));

            CreateMap<ProductOrder, OrderItem>()
                .ForMember(x => x.OneItemPrice, opt => opt.MapFrom(y => Helpers.ProductsHelpers.GetGrossPrice(y.ProductPrice, y.ProductVatRate)))
                .ForMember(x => x.Photo, opt => opt.MapFrom(y => y.Product.Photos.FirstOrDefault()))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Product.Name))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(y => y.Count));

            CreateMap<OrderDelivery, OrderDeliveryViewModel>()
                .IncludeAllDerived();
            CreateMap<PersonalPickup, OrderDeliveryViewModel>();
            CreateMap<InPostDelivery, OrderDeliveryViewModel>();
            CreateMap<Address, AddressData>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => $"{y.FirstName} {y.LastName}"))
                .ForMember(x => x.NIP, opt => opt.MapFrom(y => ""))
                .ForMember(x => x.Address1, opt => opt.MapFrom(y => y.Street))
                .ForMember(x => x.Address2, opt => opt.MapFrom(y => $"{y.ZipCode} {y.City}")); 
            
            CreateMap<Supplier, AddressData>()
                .ForMember(x => x.Address1, opt => opt.MapFrom(y => y.Street))
                .ForMember(x => x.Address2, opt => opt.MapFrom(y => $"{y.PostalCode} {y.City}"));

            CreateMap<MainPageItem, MainPageItemViewModel>();
            CreateMap<MainPageItemViewModel, EditMainPageItemViewModel>();
            CreateMap<EditMainPageItemViewModel, MainPageItem>();

        }
    }
}
