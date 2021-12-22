using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.ViewModels;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EmployeeListFilters : ListFiltersBase<EmployeeListSortOrderEnum>
    {
        public string SearchString { get; set; }
        public string RoleId { get; set; }
    }
}
