using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class WarehouseRowViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Rząd")]
        public string RowName { get; set; }
        [Required]
        public int Order { get; set; }
        public List<WarehouseSectorViewModel> Sectors { get; set; }
    }
}
