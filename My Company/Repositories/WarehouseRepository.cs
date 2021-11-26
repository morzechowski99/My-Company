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
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Warehouse>> GetAll()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Warehouse> GetById(int id)
        {
            return await FindByCondition(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Warehouse> GetWarehouse()
        {
            return await FindAll().FirstOrDefaultAsync();
        }

        public async Task<Warehouse> GetWithPlanById(int id)
        {
            return await FindByCondition(w => w.Id == id).Include(w => w.Rows).ThenInclude(r => r.Sectors).FirstOrDefaultAsync();
        }
    }
}
