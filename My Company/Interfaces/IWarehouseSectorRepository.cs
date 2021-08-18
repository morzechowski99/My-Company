using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IWarehouseSectorRepository : IRepositoryBase<WarehouseSector>
    {
        Task<bool> IsEmpty(int sectorId);
        Task<WarehouseSector> GetById(int id);
        Task<WarehouseSector> GetByWithRowId(int id);
        Task DeleteSector(WarehouseSector sector);
    }
    
}
