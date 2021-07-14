using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class ProductOrder
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
