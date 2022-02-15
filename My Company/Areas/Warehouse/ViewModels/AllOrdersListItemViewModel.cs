//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AllOrdersListItemViewModel
    {
        [Display(Name = "Numer zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
