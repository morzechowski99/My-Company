using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models.DBViews
{
    public class OrdersToComplete
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
