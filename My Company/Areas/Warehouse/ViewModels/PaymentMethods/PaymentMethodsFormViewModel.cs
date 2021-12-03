using My_Company.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
