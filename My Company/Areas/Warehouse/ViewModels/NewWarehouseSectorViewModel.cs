//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class NewWarehouseSectorViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Ilość musi być wieksza niż 0")]
        [Display(Name = "Ilość sektorów")]
        [Required]
        public int Count { get; set; }
    }
}
