using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
