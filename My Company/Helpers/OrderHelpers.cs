using My_Company.Models;
using System.Collections.Generic;
using System.Linq;

namespace My_Company.Helpers
{
    public static class OrderHelpers
    {
        public static bool CheckIfAllProductsAreAvailable(IEnumerable<ProductOrder> productOrders, out string message)
        {
            message = null;
            foreach (var po in productOrders)
            {
                if (po.Product.MagazineCount < po.Count)
                {
                    message = $"Na magazynie jest za mało produktu {po.Product.Name} ({po.Product.EANCode})\n" +
                        $"stan magazynowy: {po.Product.MagazineCount}, potrzeba {po.Count} sztuk";
                    return false;
                }
            }
            return true;
        }

        public static int GetCompletedCount(ICollection<PickingItem> pickingItems, int productOrderId)
        {
            var count = 0;
            var items = pickingItems.Where(pi => pi.ProductOrderId == productOrderId);
            foreach (var item in items)
            {
                count += item.Count;
            }
            return count;
        }

        public static bool ValidateProductCount(ProductOrder productOrder, ICollection<PickingItem> pickingItems, int count)
        {
            var prevCount = GetCompletedCount(pickingItems, productOrder.Id);
            return prevCount + count <= productOrder.Count;
        }

        public static bool CheckOrderCompleted(Order order)
        {
            foreach (var pr in order.ProductOrders)
            {
                var count = pr.Count;
                var pickingItems = order.Picking.PickingItems.Where(x => x.ProductOrderId == pr.Id).ToList();
                pickingItems.ForEach(pi => count -= pi.Count);
                if (count > 0)
                    return false;
            }
            return true;
        }

        public static bool ValidatePicking(Order order)
        {
            foreach (var pr in order.ProductOrders)
            {
                var count = pr.Count;
                var pickingItems = order.Picking.PickingItems.Where(x => x.ProductOrderId == pr.Id).ToList();
                pickingItems.ForEach(pi => count -= pi.Count);
                if (count != 0)
                    return false;
            }
            return true;
        }
    }
}
