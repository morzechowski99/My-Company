﻿using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            ProductAttributes = new HashSet<ProductAttribute>();
            Photos = new HashSet<Photo>();
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        public int MagazineCount { get; set; }
        public int Demand { get; set; }
        [MaxLength(13)]
        public string EANCode { get; set; }
        [MaxLength(15000)]
        public string Description { get; set; }
        public int SupplierId { get; set; }
        public int VATRateId { get; set; }
        public ProductStatus Status { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual VATRate VATRate { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
