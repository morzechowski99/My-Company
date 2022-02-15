//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class ChangeProductStatusViewModel
    {
        public int Id { get; set; }
        public ProductStatus Status { get; set; }
    }
}
