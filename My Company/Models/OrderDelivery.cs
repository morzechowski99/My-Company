using My_Company.EnumTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public abstract class OrderDelivery
    {
        public DeliveryType Type { get; set; }
        [Key]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
