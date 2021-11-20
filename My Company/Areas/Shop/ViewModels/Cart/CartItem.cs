using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Cart
{
    public class CartItem
    {
        public int Id { get; set; }
        [Display(Name ="Produkt")]
        public string Name { get; set; }
        public string Photo { get; set; }
        [Display(Name ="Ilość")]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Łącznie")]
        public decimal Price { get; set; }
        [Display(Name = "Cena")]
        [DataType(DataType.Currency)]
        public decimal OneItemPrice { get; set; }
    }
}
