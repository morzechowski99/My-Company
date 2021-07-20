using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class Warehouse
    {
        public Warehouse()
        {
            Sectors = new HashSet<WarehouseSector>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Voivodeship { get; set; }
        public virtual ICollection<WarehouseSector> Sectors { get; set; }
    }
}
