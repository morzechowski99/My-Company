//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Shop.Enums;
using My_Company.ViewModels;
using System.Collections.Generic;

namespace My_Company.Areas.Shop.ViewModels.Products
{
    public class ProductsListFilters : ListFiltersBase<ProductSortEnum>
    {
        public int? CategoryId { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public List<AttributeListValue> Attributes { get; set; }
    }
}
