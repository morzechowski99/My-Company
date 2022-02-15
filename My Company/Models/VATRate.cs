//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;

namespace My_Company.Models
{
    public class VATRate
    {
        public VATRate()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int Rate { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
