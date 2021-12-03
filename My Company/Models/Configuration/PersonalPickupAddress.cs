using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models.Configuration
{
    public class PersonalPickupAddress
    {
    
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^\d{2}-\d{3}", ErrorMessage = "Zły format")]
        [MaxLength(6)]
        public string ZipCode { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Nieprawidłowy numer telefonu")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}
