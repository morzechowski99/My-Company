//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Nazwa kategorii")]
        [Required]
        [MaxLength(50)]
        [Remote(action: "CheckName", controller: "Categories", areaName: "Warehouse")]
        public string CategoryName { get; set; }
        [Display(Name = "Opis")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string Description { get; set; }
        [Display(Name = "Kategoria nadrzędna")]
        [Required]
        [Range(-1, Int32.MaxValue, ErrorMessage = "Pole wymagane")]
        public int? ParentCategoryId { get; set; }
        [Required]
        public List<CreteAttributeViewModel> Attributes { get; set; }

    }
}
