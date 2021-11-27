using My_Company.EnumTypes;
using My_Company.Models;
using System;
using System.Threading.Tasks;

namespace My_Company.Services.PaymentService
{
    [PaymentType(PaymentMethodEnum.TraditionalTransfer)]
    public class TraditionalTransferService : IPaymentService
    {
        public async Task<string> GetLinkToPayment(Order order)
        {
            return null;
        }
    }
}
