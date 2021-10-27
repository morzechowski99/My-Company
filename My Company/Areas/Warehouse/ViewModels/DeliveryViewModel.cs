using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryViewModel
    {
        [Display(Name = "Dostawca")]
        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }
        [Display(Name = "Produkty")]
        [MinLength(1,ErrorMessage ="Dodaj co najmniej jeden produkt")]
        public List<DeliveryItemViewModel> Items { get; set; }
    }
}
