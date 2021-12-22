using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa kategorii")]
        public string CategoryName { get; set; }
        [Display(Name = "Opis")]
        public string Descripttion { get; set; }
        [Display(Name = "Drzewo kategorii")]
        public string CategoryTree { get; set; }
        [Display(Name = "Atrybuty")]
        public List<AttributeViewModel> Attributes { get; set; }
    }
}
