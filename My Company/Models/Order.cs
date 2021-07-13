using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Order
    {
        public Order()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
        public OrderStatus Status { get; set; }
        public bool Paid { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
