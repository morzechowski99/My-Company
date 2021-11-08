using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class PickingItemsRepository : RepositoryBase<PickingItem>, IPickingItemsRepository
    {
        public PickingItemsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PickingItem> GetItemById(int id)
        {
            return await FindByCondition(pi => pi.Id == id)
                .Include(pi => pi.ProductOrder)
                .FirstOrDefaultAsync();
        }
    }
}
