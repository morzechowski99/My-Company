﻿using My_Company.EnumTypes;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CreteAttributeViewModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Typ")]
        public AttributeType Type { get; set; }
    }
}
