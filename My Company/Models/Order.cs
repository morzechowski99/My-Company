using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Comment { get; set; }
        public OrderStatus Status { get; set; }
        public bool Paid { get; set; }
        public int AddressId { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public int PaymentPrice { get; set; }
        public int DeliveryPrice { get; set; }
        public string Email { get; set; }
        public string InvoiceNumber { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual Picking Picking { get; set; }
        public virtual Address Address { get; set; }
        public virtual OrderDelivery Delivery { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Packing Packing { get; set; }
    }
}
