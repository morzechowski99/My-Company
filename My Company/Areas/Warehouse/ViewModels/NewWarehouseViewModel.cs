using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class NewWarehouseViewModel
    {
        [Required]
        [Display(Name ="Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Województwo")]
        public string Voivodeship { get; set; }
        public List<NewWarehouseSectorViewModel> Sectors { get; set; }

    }
}
