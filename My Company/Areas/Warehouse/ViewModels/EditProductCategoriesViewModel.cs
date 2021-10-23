using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditProductCategoriesViewModel
    {
        public int Id { get; set; }
        public List<int> Categories { get; set; }
    }
}
