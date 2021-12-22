using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderPackingViewModel
    {
        [Display(Name = "Numer zamówienia")]
        public Guid Id { get; set; }
        public List<ProductListItemViewModel> ProductOrders { get; set; }

    }
}
