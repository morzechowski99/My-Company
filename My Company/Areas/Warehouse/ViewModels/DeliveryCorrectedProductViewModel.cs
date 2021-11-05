using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryCorrectedProductViewModel
    {
        public DeliveryItemViewModel Orginal { get; set; }
        public DeliveryProductViewModel AfterCorrection { get; set; }
    }
}
