using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AddProductToPickingViewModel
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int SectorId { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Count { get; set; }
    }
}
