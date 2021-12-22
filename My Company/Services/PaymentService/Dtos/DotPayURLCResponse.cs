using System;

namespace My_Company.Services.PaymentService.Dtos
{
    public class DotPayURLCResponse
    {
        public int Id { get; set; }
        public string Operation_number { get; set; }
        public string Operation_type { get; set; }
        public string Operation_status { get; set; }
        public string Operation_amount { get; set; }
        public string Operation_currency { get; set; }
        public string Operation_withdrawal_amount { get; set; }
        public string Operation_commission_amount { get; set; }
        public string Operation_original_amount { get; set; }
        public string Operation_original_currency { get; set; }
        public DateTime Operation_datetime { get; set; }
        public string Operation_related_number { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public Guid Control { get; set; }

    }
}
