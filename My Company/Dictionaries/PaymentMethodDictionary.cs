using My_Company.EnumTypes;
using System.Collections.Generic;

namespace My_Company.Dictionaries
{
    public class PaymentMethodDictionary
    {

        private static Dictionary<PaymentMethodEnum, string> paymentMethodsDictionary = new()
        {
            { PaymentMethodEnum.TraditionalTransfer, "Przelew tradycyjny" },
            { PaymentMethodEnum.DotPay, "Szybki przelew DotPay" },
        };
        public static Dictionary<PaymentMethodEnum, string> PaymentDictionary { get { return paymentMethodsDictionary; } }
    }
}
