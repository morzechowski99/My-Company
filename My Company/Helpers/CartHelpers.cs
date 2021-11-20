using My_Company.Areas.Shop.ViewModels.Cart;
using System.Collections.Generic;

namespace My_Company.Helpers
{
    public static class CartHelpers
    {
        public static decimal GetCartTotal(List<CartItem> items)
        {
            decimal total = 0.0M;
            items.ForEach(i => total += i.Price);
            return total;
        }
    }
}
