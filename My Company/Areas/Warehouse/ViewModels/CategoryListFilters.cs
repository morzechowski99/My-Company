using My_Company.Areas.Warehouse.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CategoryListFilters : ListFiltersBase<CategoriesSortOrderEnum>
    {
        public string SearchString { get; set; }
    }
}
