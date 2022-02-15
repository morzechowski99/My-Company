//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;

namespace My_Company.Models.Configuration
{
    public class PaymentMethod
    {
        public int Price { get; set; }
        public PaymentMethodEnum Method { get; set; }
    }
}
