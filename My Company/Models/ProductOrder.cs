//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;

namespace My_Company.Models
{
    public class ProductOrder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int Count { get; set; }
        public int ProductPrice { get; set; }
        public int ProductVatRate { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
