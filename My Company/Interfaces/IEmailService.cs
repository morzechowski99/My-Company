using My_Company.EnumTypes;
using My_Company.Models;

namespace My_Company.Interfaces
{
    public interface IEmailService
    {
        void SendOrderEmail(OrderEmailReason reason, Order order, string email = null);
        void SendRegistrationEmail(string email, string url);
    }
}
