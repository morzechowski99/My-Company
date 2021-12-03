using My_Company.EnumTypes;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels.PaymentMethods
{
    public class PaymentMethodViewModel
    {
        [Display(Name = "Cena")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^(\d*\,\d{1,2}|\d+)$")]
        public string Price { get; set; }
        public PaymentMethodEnum Method { get; set; }
        public bool Enabled { get; set; }
    }
}
