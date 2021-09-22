using My_Company.EnumTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class AttributeDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Typ")]
        public AttributeType Type { get; set; }
        [Display(Name = "Wartości")]
        public IEnumerable<string> Values { get; set; }
    }
}