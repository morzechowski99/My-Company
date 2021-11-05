using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IDeliveriesRepository : IRepositoryBase<Delivery>
    {
        ICollection<ProductDelivery> RemoveDuplicates(List<ProductDelivery> productDeliveries);
        IQueryable<Delivery> GetDeliveriesByFilters(DeliveriesListFilters filters);
        Task<string> CreatePZNumber();
        Task<Delivery> GetDeliveryById(int id);
        Task<string> CreateKPZNumber();
        Task<Delivery> GetDeliveryCorrectedDeliveryById(int id);
    }
}
