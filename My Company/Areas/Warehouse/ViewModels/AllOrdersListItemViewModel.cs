using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AllOrdersListItemViewModel
    {
        [Display(Name ="Numer zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
