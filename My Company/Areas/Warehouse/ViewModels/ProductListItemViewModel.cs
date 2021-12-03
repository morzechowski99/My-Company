using My_Company.Areas.Warehouse.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ProductListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StockState StockStatus { get; set; }
        public string PhotoUrl { get; set; }
        [Display(Name="Dostawca")]
        public string SupplierData { get; set; }
        [Display(Name ="EAN")]
        public string EANCode { get; set; }
        [Display(Name ="Ilość")]
        public int Count { get; set; }
    }
}
