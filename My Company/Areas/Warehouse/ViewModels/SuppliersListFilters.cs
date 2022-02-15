//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.ViewModels;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class SuppliersListFilters : ListFiltersBase<SuppliersListSortOrderEnum>
    {
        public string SearchString { get; set; }
        public string Nip { get; set; }
        public string Email { get; set; }
    }
}
