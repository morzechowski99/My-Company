using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AddSectorsViewModel
    {
        [Required]
        [Range(1,Int32.MaxValue,ErrorMessage = "Ilość musi być wieksza niż 0")]
        [Display(Name ="Ilość")]
        public int Count { get; set; }
        public int RowId { get; set; } 
    }
}
