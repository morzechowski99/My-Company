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
    public class WarehouseRowRepository : RepositoryBase<WarehouseRow>, IWarehouseRowRepository
    {
        public WarehouseRowRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task DeleteRow(WarehouseRow row)
        {
            var rows = await FindByCondition(r => r.WarehouseId == row.WarehouseId && r.Order > row.Order).ToListAsync();
            foreach(var item in rows)
            {
                item.Order = item.Order - 1;
                Update(item);
            }
            Delete(row);
        }

        public async Task<WarehouseRow> GetById(int rowId)
        {
            return await FindByCondition(row => row.Id == rowId).Include(row => row.Sectors).FirstOrDefaultAsync();
        }

        public async Task<WarehouseRow> GetByOrderAndWarehouse(int order, int warehouseId)
        {
            return await FindByCondition(row => row.Order == order && row.WarehouseId == warehouseId).FirstOrDefaultAsync();
        }
    }
}
