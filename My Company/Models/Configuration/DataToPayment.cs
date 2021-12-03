using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models.Configuration
{
    public class DataToPayment
    {
        [Display(Name ="Nazwa firmy")]
        public string CompanyName { get; set; }
        [Display(Name = "Numer konta")]
        public string AccountNumber { get; set; }
        [Display(Name = "Nazwa banku")]
        public string BankName { get; set; }
    }
}
