using Newtonsoft.Json;

namespace My_Company.Services.PaymentService.Dtos
{
    public class DotPayCreatePaymentRequest
    {
        [JsonProperty("api_version")]
        public string Api_Version { get; set; } = "next";
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; } = "PLN";
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("chk")]
        public string Chk { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("urlc")]
        public string UrlC { get; set; }
        [JsonProperty("buttontext")]
        public string ButtonText { get; set; } = "Wróć do sklepu";
        [JsonProperty("control")]
        public string Control { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; } = "pl";




    }
}
