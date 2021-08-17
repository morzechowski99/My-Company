using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IWarehouseRowRepository: IRepositoryBase<WarehouseRow>
    {
        Task<WarehouseRow> GetById(int rowId);
        Task<WarehouseRow> GetByOrderAndWarehouse(int order, int warehouseId);
        Task DeleteRow(WarehouseRow row);
    }
}
