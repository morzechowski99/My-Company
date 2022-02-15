//Program powstał na Wydziale Informatyki Politechniki Białostockiej
namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryCorrectedProductViewModel
    {
        public DeliveryItemViewModel Orginal { get; set; }
        public DeliveryProductViewModel AfterCorrection { get; set; }
    }
}
