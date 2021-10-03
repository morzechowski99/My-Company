using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class SupplierListItem
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [MaxLength(15)]
        [Display(Name = "NIP")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "NIP musi miec 10 cyfr")]
        public string NIP { get; set; }
        [Display(Name = "Numer telefonu 1")]
        public string PhoneNumber1 { get; set; }
        [Display(Name = "Strona Internetowa")]
        public string WebSite { get; set; }
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
        public bool Deletable { get; set; }
    }
}
