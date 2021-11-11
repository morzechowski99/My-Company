using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ProductOrderDetailsViewModel
    {
        [Display(Name = "Ilość")]
        public int Count { get; set; }
        [Display(Name = "Prodkt")]
        public string ProductDescritpion { get; set; }
    }
}
