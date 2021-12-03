using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;

namespace My_Company.Repositories
{
    public class OrderPackingRepository : RepositoryBase<Packing>, IOrderPackingRepository
    {
        public OrderPackingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
