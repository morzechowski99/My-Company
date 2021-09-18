using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Product
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
            ProductCategories = new HashSet<ProductCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MagazineCount { get; set; }
        public int Demand { get; set; }
        public string EANCode { get; set; }
        public string Description { get; set; }
        public int SupplierId { get; set; }
        public int VATRateId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual VATRate VATRate { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
