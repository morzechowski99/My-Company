//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.ViewModels;
using System;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveriesListFilters : ListFiltersBase<DeliveriesSortOrderEnum>
    {
        public int? SupplierId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string PZNumber { get; set; }
    }
}
