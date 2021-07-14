using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class ProductSector
    {
        public int SectorId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public virtual WarehouseSector Sector { get; set; }
        public virtual Product Product { get; set; }
    }
}
