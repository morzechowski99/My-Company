using My_Company.Areas.Shop.Enums;
using My_Company.ViewModels;

namespace My_Company.Areas.Shop.ViewModels.Products
{
    public class ProductsListFilters : ListFiltersBase<ProductSortEnum>
    {
        public int? CategoryId { get; set; }
    }
}
