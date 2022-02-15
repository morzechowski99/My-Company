//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AttributeValuesViewModel
    {
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
}
