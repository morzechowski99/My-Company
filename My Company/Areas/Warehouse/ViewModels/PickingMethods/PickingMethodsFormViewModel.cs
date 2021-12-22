using My_Company.Models.Configuration;
using System.Collections.Generic;

namespace My_Company.Areas.Warehouse.ViewModels.PickingMethods
{
    public class PickingMethodsFormViewModel
    {
        public List<PickingMethodViewModel> Methods { get; set; }
        public PersonalPickupAddress Addres { get; set; }
    }
}
