using My_Company.EnumTypes;
using My_Company.Models;

namespace My_Company.Interfaces
{
    public interface IEmailService
    {
        void SendOrderEmail(EmailReason reason, Order order, string email = null);
    }
}
