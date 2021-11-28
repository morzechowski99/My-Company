using My_Company.Models;
using static My_Company.Dictionaries.OrderStatusesDictionary;

namespace My_Company.EnumTypes
{
    public enum OrderEmailReason
    {
        NewOrder,
        ChangeOrderStatus
    }

    public static class EmailReasonExtensions
    {
        public static string GetEmailTitle(this OrderEmailReason reason, Order order)
        {
            return reason switch
            {
                var r when r == OrderEmailReason.NewOrder => $"Nowe zamówienie nr {order.Id}",
                var r when r == OrderEmailReason.ChangeOrderStatus => $"Zmiana statusu zamówienia nr {order.Id}",
                _ => null
            };
        }       
        
        public static string GetEmailContent(this OrderEmailReason reason, Order order)
        {
            return reason switch
            {
                var r when r == OrderEmailReason.NewOrder => $"Dziękujemy za złożenie zamówienia ",
                var r when r == OrderEmailReason.ChangeOrderStatus => $"Status twojego zamówienia został zmieniony na {Dictionary[order.Status]}",
                _ => null
            };
        }
    }
}
