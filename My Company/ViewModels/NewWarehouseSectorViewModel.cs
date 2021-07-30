using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.ViewModels
{
    public class NewWarehouseSectorViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Range(1,Int32.MaxValue,ErrorMessage ="Ilość musi być wieksza niż 0")]
        [Display(Name = "Ilość sektorów")]
        [Required]
        public int Count { get; set; }
    }
}
