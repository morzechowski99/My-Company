//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Products
{
    public class ListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
