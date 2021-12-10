using My_Company.EnumTypes;
using My_Company.Models;

namespace My_Company.Interfaces
{
    public interface IEmailService
    {
        void SendOrderEmail(OrderEmailReason reason, Order order, string url, string email = null);
        void SendRegistrationEmail(string email, string url);
        void SendEmail(string email, string title, string content);
    }
}
