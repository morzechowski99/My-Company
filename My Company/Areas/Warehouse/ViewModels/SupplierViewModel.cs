using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name="Nazwa")]
        public string Name { get; set; }
        [MaxLength(15)]
        [Display(Name = "NIP")]
        [RegularExpression(@"^\d{10}$",ErrorMessage ="NIP musi miec 10 cyfr")]
        public string NIP { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required]
        [MaxLength(6)]
        [Display(Name = "Kod pocztowy")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{2}-\d{3}",ErrorMessage ="Zły format")]
        public string PostalCode { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [MaxLength(15)]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu 1")]
        [RegularExpression(@"(?<!\w)(\(?(\+)?\d{1,3}\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)")]
        public string PhoneNumber1 { get; set; }
        [MaxLength(15)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu 2")]
        [RegularExpression(@"(?<!\w)(\(?(\+)?\d{1,3}\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)")]
        public string PhoneNumber2 { get; set; }
        [MaxLength(50)]
        [DataType(DataType.Url)]
        [Display(Name = "Strona Internetowa")]
        public string WebSite { get; set; }
        [MaxLength(50)]
        [DataType(DataType.EmailAddress,ErrorMessage ="Niepoprawny adres e-mail")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
    }
}
