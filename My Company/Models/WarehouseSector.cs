using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class WarehouseSector
    {
        public int Id { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public int SectorId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
