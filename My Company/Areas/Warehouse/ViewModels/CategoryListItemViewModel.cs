//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CategoryListItemViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string CategoryName { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Drzewo kategorii")]
        public string CategoryTree { get; set; }
        public bool Deletable { get; set; }
    }
}
