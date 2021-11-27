using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services.PaymentService
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PaymentTypeAttribute : Attribute
    {
        public PaymentTypeAttribute(PaymentMethodEnum type)
        {
            this.PaymentMethod = type;
        }

        public PaymentMethodEnum PaymentMethod { get; }
    }
}
