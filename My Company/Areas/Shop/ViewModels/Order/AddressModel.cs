using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Order
{
    public class AddressModel
    {
        [Required]
        [Display(Name ="Imię")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^\d{2}-\d{3}", ErrorMessage = "Zły format")]
        [MaxLength(6)]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Nieprawidłowy numer telefonu")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}