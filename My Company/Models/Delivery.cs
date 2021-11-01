using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class Delivery
    {
        public Delivery()
        {
            ProductDeliveries = new HashSet<ProductDelivery>();
        }

        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateTime DeliveryDate { get; set; }
        [Required]
        public string PZNumber { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductDelivery> ProductDeliveries { get; set; }
    }
}
