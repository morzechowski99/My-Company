using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ConfigValue
    {
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
