using DataAnnotationsExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Display(Name ="Nazwa kategorii")]
        [Required]
        [MaxLength(50)]
        [Remote(action:"CheckName",controller:"Categories",areaName:"Warehouse")]
        public string CategoryName { get; set; }
        [Display(Name ="Opis")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string Descripttion { get; set; }
        [Display(Name ="Kategoria nadrzędna")]
        [Required]
        [Range(-1,Int32.MaxValue,ErrorMessage = "Pole wymagane")]
        public int? ParentCategoryId { get; set; }
        [Required]
        public List<CreteAttributeViewModel> Attributes { get; set; }

    }
}
