using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class NewProductViewModel
    {
        [Display(Name ="Nazwa")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Kod EAN")]
        [RegularExpression(@"^\d$",ErrorMessage ="Pole może zawierać tylko cyfry")]
        [Required]
        public string EANCode { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        [Display(Name = "Dostawca")]
        [Required]
        public int SupplierId { get; set; }
        [Display(Name = "Stawka VAT")]
        [Required]
        public int VATRateId { get; set; }
    }
}
