using Microsoft.EntityFrameworkCore.Storage;
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
        Task Save();
        Task<IDbContextTransaction> BeginTransaction();
        bool EnsureCreated();
    }
}
