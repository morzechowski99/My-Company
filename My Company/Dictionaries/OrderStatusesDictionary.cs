using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Dictionaries
{
    public class OrderStatusesDictionary
    {
        private static Dictionary<OrderStatus, string> orderStatusesDictionary = new() {
            { OrderStatus.New, "Złożone" },
            { OrderStatus.Paid, "Opłacone" },
            { OrderStatus.Completed, "Skompletowane" },
            { OrderStatus.Send, "Wysłane" },         
        };

        public static Dictionary<OrderStatus, string> Dictionary { get { return orderStatusesDictionary; } }
    }
}
