using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
