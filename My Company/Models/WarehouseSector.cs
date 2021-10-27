using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Models
{
    public class WarehouseSector
    {
        public WarehouseSector()
        {
            ProductDeliveries = new HashSet<ProductDelivery>();
        }

        public int Id { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int RowId { get; set; }
        public virtual WarehouseRow Row { get; set; }
        public virtual ICollection<ProductDelivery> ProductDeliveries { get; set; }
    }
}
