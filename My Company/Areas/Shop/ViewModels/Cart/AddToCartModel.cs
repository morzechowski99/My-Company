//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Cart
{
    public class AddToCartModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}
