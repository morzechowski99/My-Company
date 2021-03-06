using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IWarehouseSectorRepository : IRepositoryBase<WarehouseSector>
    {
        Task<bool> IsEmpty(int sectorId);
        Task<WarehouseSector> GetById(int id);
        Task<WarehouseSector> GetByIdWithRow(int id);
        Task DeleteSector(WarehouseSector sector);
        Task<IEnumerable<WarehouseSector>> GetSectorsByRow(int rowId);
        Task<IEnumerable<WarehouseSector>> GetAll();
    }

}
