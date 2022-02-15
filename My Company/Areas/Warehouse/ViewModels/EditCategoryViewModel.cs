//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditCategoryViewModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Nazwa kategorii")]
        [Required]
        [MaxLength(50)]
        [Remote(action: "CheckNameEdit", controller: "Categories", areaName: "Warehouse", AdditionalFields = "Id")]
        public string CategoryName { get; set; }
        [Display(Name = "Opis")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string Description { get; set; }

    }
}
