using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AttributeValuesViewModel
    {
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
}
