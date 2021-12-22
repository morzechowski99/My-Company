using System;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderListItemViewModel
    {
        [Display(Name = "Id zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data zamówienia")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public bool? PickingStarted { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public bool? PackingStarted { get; set; }
    }
}
