//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels.PickingMethods
{
    public class PickingMethodViewModel
    {
        [Display(Name = "Cena")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^(\d*\,\d{1,2}|\d+)$")]
        public string Price { get; set; }
        public DeliveryType Type { get; set; }
        public bool Enabled { get; set; }
    }
}
