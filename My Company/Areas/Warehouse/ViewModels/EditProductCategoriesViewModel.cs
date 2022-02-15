//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditProductCategoriesViewModel
    {
        public int Id { get; set; }
        public List<int> Categories { get; set; }
    }
}
