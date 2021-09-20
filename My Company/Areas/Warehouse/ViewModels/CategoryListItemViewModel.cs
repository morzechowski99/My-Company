using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CategoryListItemViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Nazwa")]
        public string CategoryName { get; set; }
        [Display(Name ="Opis")]
        public string Descripttion { get; set; }
        [Display(Name ="Drzewo kategorii")]
        public string CategoryTree { get; set; }
    }
}
