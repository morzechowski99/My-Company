using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services.PaymentService.Dtos
{
    public class DotPayCreatePaybentRequest
    {
        [JsonProperty("api_version")]
        public string ApiVersion { get; set; } = "next";
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
        public string Language { get; set; } = "pl";




    }
}
