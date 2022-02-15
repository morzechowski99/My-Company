//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System;

namespace My_Company.Services.DeliveryService
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DeliveryTypeAttribute : Attribute
    {
        public DeliveryTypeAttribute(DeliveryType type)
        {
            this.DeliveryType = type;
        }

        public DeliveryType DeliveryType { get; }
    }
}
