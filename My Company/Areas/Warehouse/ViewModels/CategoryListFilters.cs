using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.ViewModels;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CategoryListFilters : ListFiltersBase<CategoriesSortOrderEnum>
    {
        public string SearchString { get; set; }
    }
}
