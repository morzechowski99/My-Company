using My_Company.Models;
using static My_Company.Dictionaries.OrderStatusesDictionary;

namespace My_Company.EnumTypes
{
    public enum EmailReason
    {
        NewOrder,
        ChangeOrderStatus
    }

    public static class EmailReasonExtensions
    {
        public static string GetEmailTitle(this EmailReason reason, Order order)
        {
            return reason switch
            {
                var r when r == EmailReason.NewOrder => $"Nowe zamówienie nr {order.Id}",
                var r when r == EmailReason.ChangeOrderStatus => $"Zmiana statusu zamówienia nr {order.Id}",
                _ => null
            };
        }       
        
        public static string GetEmailContent(this EmailReason reason, Order order)
        {
            return reason switch
            {
                var r when r == EmailReason.NewOrder => $"Dziękujemy za złożenie zamówienia ",
                var r when r == EmailReason.ChangeOrderStatus => $"Status twojego zamówienia został zmieniony na {Dictionary[order.Status]}",
                _ => null
            };
        }
    }
}
