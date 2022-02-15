//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.EnumTypes;
using My_Company.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Products
{
    public class ProductDetailsPageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Kategoria")]
        public string Category { get; set; }
        public int CategoryId { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name = "Dane produktu")]
        public List<AttributeProductViewModel> Attributes { get; set; }
        public List<string> Photos { get; set; }
        [Display(Name = "Dostępność")]
        public StockState State { get; set; }
        public ProductStatus Status { get; set; }
        public List<CategoryNameAndId> ProductCategories { get; set; }
    }
}
