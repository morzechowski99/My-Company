using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ChangeProductStatusViewModel
    {
        public int Id { get; set; }
        public ProductStatus Status { get; set; }
    }
}
