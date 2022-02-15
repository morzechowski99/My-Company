//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;

namespace My_Company.Models
{
    public class PickingItem
    {
        public int Id { get; set; }
        public Guid PickingId { get; set; }
        public int ProductOrderId { get; set; }
        public int SectorId { get; set; }
        public int Count { get; set; }
        public virtual Picking Picking { get; set; }
        public virtual ProductOrder ProductOrder { get; set; }
        public virtual WarehouseSector Sector { get; set; }
    }
}
