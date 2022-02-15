//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System;

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
