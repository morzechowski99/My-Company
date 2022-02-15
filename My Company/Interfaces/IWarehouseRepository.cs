//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IWarehouseRepository : IRepositoryBase<Warehouse>
    {
        Task<List<Warehouse>> GetAll();
        Task<Warehouse> GetById(int id);
        Task<Warehouse> GetWithPlanById(int id);
        Task<Warehouse> GetWarehouse();
    }
}
