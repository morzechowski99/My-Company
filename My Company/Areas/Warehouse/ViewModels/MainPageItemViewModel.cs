using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class MainPageItemViewModel
    {
        public int Order { get; set; }
        public string ButtonText { get; set; }
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
    }
}
