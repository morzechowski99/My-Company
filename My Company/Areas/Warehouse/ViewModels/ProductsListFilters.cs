using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.EnumTypes;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ProductsListFilters : ListFiltersBase<ProductsSortOrderEnum>
    {
        public string SearchString { get; set; }
        public List<StockState> States { get; set; }
        public List<string> Eans { get; set; }
        public List<int> Suppliers { get; set; }
        public List<ProductStatus> Statuses { get; set; }
        public List<int> Categories { get; set; }
    }
}
