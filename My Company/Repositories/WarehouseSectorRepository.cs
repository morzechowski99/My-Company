using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class WarehouseSectorRepository : RepositoryBase<WarehouseSector>, IWarehouseSectorRepository
    {
        public WarehouseSectorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsEmpty(int sectorId)
        {
            return await FindByCondition(s => s.Id == sectorId).AnyAsync();
        }

        public async Task<WarehouseSector> GetById(int id)
        {
            return await FindByCondition(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<WarehouseSector> GetByIdWithRow(int id)
        {
            return await FindByCondition(s => s.Id == id).Include(s => s.Row).FirstOrDefaultAsync();
        }

        public async Task DeleteSector(WarehouseSector sector)
        {
            var sectors = await FindByCondition(s => s.RowId == sector.RowId).ToListAsync();
            foreach (var item in sectors)
            {
                if (item.Order > sector.Order)
                {
                    item.Order = item.Order - 1;
                    Update(item);
                }
            }
            Delete(sector);
        }

        public async Task<IEnumerable<WarehouseSector>> GetSectorsByRow(int rowId)
        {
            return await FindByCondition(s => s.RowId == rowId).ToListAsync();
        }

        public async Task<IEnumerable<WarehouseSector>> GetAll()
        {
            return await FindAll().ToListAsync();
        }
    }
}
