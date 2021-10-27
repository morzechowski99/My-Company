namespace My_Company.Models
{
    public class ProductDelivery
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public int SectorId { get; set; }
        public int Count { get; set; }
        public virtual Product Product { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual WarehouseSector Sector { get; set; }
    }
}
