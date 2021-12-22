namespace My_Company.Areas.Warehouse.ViewModels
{
    public class WarehouseSectorViewModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public bool Deletable { get; set; }
        public string Barcode { get; set; }

    }
}
