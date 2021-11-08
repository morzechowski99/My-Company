using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderPickingViewModel
    {
        public Guid Id { get; set; }
        public List<OrderPickingItemViewModel> Items { get; set; }
        public List<PickedItemViewModel> PickedItems { get; set; }
    }
}
