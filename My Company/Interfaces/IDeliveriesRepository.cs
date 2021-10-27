using My_Company.Models;
using System.Collections.Generic;

namespace My_Company.Interfaces
{
    public interface IDeliveriesRepository : IRepositoryBase<Delivery>
    {
        ICollection<ProductDelivery> RemoveDuplicates(List<ProductDelivery> productDeliveries);
    }
}
