//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Threading.Tasks;

namespace My_Company.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<string> GetLinkToPayment(Order order);
    }
}
