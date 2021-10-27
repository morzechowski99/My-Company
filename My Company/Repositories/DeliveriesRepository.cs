using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;

namespace My_Company.Repositories
{
    public class DeliveriesRepository : RepositoryBase<Delivery>, IDeliveriesRepository
    {
        public DeliveriesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ICollection<ProductDelivery> RemoveDuplicates(List<ProductDelivery> productDeliveries)
        {
            var copy = new List<ProductDelivery>(productDeliveries);
            int i = 0;
            foreach (var item in productDeliveries)
            {
                for (int j = i + 1; j < productDeliveries.Count; j++)
                {
                    var pd = productDeliveries[j];
                    if (pd.ProductId == item.ProductId && pd.SectorId == item.SectorId)
                    {
                        item.Count += pd.Count;
                        copy.Remove(pd);
                    }
                }
                i++;
            }
            return copy;
        }
    }
}
