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
        public virtual AppUser User { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual Picking Picking { get; set; }
    }
}
