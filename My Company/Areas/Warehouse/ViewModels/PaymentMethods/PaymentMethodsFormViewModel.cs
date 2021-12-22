using My_Company.Models.Configuration;
using System.Collections.Generic;

namespace My_Company.Areas.Warehouse.ViewModels.PaymentMethods
{
    public class PaymentMethodsFormViewModel
    {
        public List<PaymentMethodViewModel> Methods { get; set; }
        public int? DotPayId { get; set; }
        public string DotPayPin { get; set; }
        public DataToPayment DataToPayment { get; set; }
    }
}
