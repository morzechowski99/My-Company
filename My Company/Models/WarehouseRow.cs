using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class WarehouseRow
    {
        public WarehouseRow()
        {
            Sectors = new HashSet<WarehouseSector>();
        }

        public int Id { get; set; }
        [Required]
        public string RowName { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        [Required]
        public int Order { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<WarehouseSector> Sectors { get; set; }

    }
}
