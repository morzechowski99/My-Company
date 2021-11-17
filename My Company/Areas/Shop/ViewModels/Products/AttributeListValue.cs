using My_Company.EnumTypes;

namespace My_Company.Areas.Shop.ViewModels.Products
{
    public class AttributeListValue
    {
        public int Id { get; set; }
        public AttributeType Type { get; set; }
        public string Value { get; set; }
        public string ValueFrom { get; set; }
        public string ValueTo { get; set; }
        public string[] Values { get; set; }
    }
}
