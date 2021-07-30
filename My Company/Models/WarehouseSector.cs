using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class WarehouseSector
    {
        public int Id { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int RowId { get; set; }
        public virtual WarehouseRow Row { get; set; }
    }
}
