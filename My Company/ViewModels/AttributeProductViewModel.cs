using My_Company.EnumTypes;
using System.Collections.Generic;

namespace My_Company.ViewModels
{
    public class AttributeProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AttributeType Type { get; set; }
        public string Value { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}