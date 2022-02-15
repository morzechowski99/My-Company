//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Profile
{
    public class UserOrderListItemViewModel
    {
        [Display(Name = "Nr. zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Kwota")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
    }
}
