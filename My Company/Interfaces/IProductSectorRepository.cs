//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IProductSectorRepository : IRepositoryBase<ProductSector>
    {
        Task<ProductSector> GetByProductAndSector(int productId, int sectorId);
    }
}
