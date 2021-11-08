using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class PickingRepository : RepositoryBase<Picking>, IPickingRepository
    {
        public PickingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<PickingItem>> GetItems(Guid pickingId)
        {
            var picking = await FindByCondition(p => p.OrderId == pickingId)
                .Include(p => p.PickingItems)
                .ThenInclude(pi => pi.ProductOrder.Product.ProductSectors)
                .ThenInclude(ps => ps.Sector.Row)
                .FirstOrDefaultAsync();
            if (picking == null)
            {
                return null;
            }

            return picking.PickingItems.ToList();
        }
    }
}
