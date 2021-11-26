using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewModels.Cart
{
    public class Cart
    {
        public Cart()
        {
            Items = new();
        }
        [DataType(DataType.Currency)]
        [Display(Name ="Suma")]
        public decimal Total { get; set; }
        [Display(Name ="Produkty")]
        public List<CartItem> Items { get; set; }
    }
}
