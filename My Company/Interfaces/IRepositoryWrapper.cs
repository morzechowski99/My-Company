using Microsoft.EntityFrameworkCore.Storage;
using My_Company.Models;
using System;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IRepositoryWrapper
    {
        IWarehouseRepository WarehouseRepository { get; }
        IWarehouseRowRepository WarehouseRowRepository { get; }
        IWarehouseSectorRepository WarehouseSectorRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        IVATRatesRepository VATRatesRepository { get; }
        ISuppliersRepository SuppliersRepository { get; }
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        ICategoryAttributesRepository CategoryAttributesRepository { get; }
        IProductAttributeRepository ProductAttributeRepository { get; }
        IPhotosRepository PhotosRepository { get; }
        IDeliveriesRepository DeliveriesRepository { get; }
        IProductSectorRepository ProductSectorRepository { get; }
        IOrdersToCompleteView OrdersToCompleteView { get; }
        IOrdersRepository OrdersRepository { get; }
        IPickingRepository PickingRepository { get; }
        IPickingItemsRepository PickingItemsRepository { get; }
        IConfigRepository ConfigRepository { get; }
        IAddressesRepository AddressesRepository { get; }
        Task Save();
        Task<IDbContextTransaction> BeginTransaction();
        bool EnsureCreated();
        void ClearTracked();
        
    }
}
