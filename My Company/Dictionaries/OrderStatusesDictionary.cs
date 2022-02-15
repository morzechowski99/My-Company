//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System.Collections.Generic;

namespace My_Company.Dictionaries
{
    public class OrderStatusesDictionary
    {
        private static Dictionary<OrderStatus, string> orderStatusesDictionary = new()
        {
            { OrderStatus.New, "Złożone" },
            { OrderStatus.Paid, "Opłacone" },
            { OrderStatus.Completed, "Skompletowane" },
            { OrderStatus.Send, "Wysłane" },
            { OrderStatus.Ready, "Gotowe do odbioru" },
        };

        public static Dictionary<OrderStatus, string> Dictionary { get { return orderStatusesDictionary; } }
    }
}
