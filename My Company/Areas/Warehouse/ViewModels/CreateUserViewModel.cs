﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CreateUserViewModel
    {
        [Display(Name="Imię")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [EmailAddress]
        [Required]
        [MaxLength(60)]
        [Display(Name = "Adres Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Rola")]
        public string Role { get; set; }
    }
}
