//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class ProductSectorRepository : RepositoryBase<ProductSector>, IProductSectorRepository
    {
        public ProductSectorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProductSector> GetByProductAndSector(int productId, int sectorId)
        {
            return await FindByCondition(ps => ps.ProductId == productId && ps.SectorId == sectorId)
                .FirstOrDefaultAsync();
        }
    }
}
