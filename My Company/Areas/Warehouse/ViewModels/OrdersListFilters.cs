using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.EnumTypes;
using My_Company.ViewModels;
using System;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrdersListFilters : ListFiltersBase<OrdersSortOrderEnum>
    {
        public Guid? OrderId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public OrderStatus[] Statuses { get; set; }
    }

}
