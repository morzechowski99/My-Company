using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using My_Company.EnumTypes;
using My_Company.Extensions;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Models.AppSettings;
using My_Company.Services.PaymentService.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static My_Company.Helpers.Constants;
using static My_Company.Helpers.OrderHelpers;

namespace My_Company.Services.PaymentService
{
    [PaymentType(PaymentMethodEnum.DotPay)]
    public class DotPayService : IPaymentService
    {
        private readonly DotPayOptions dotPayOptions;
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly HttpContext httpContext;
        private readonly string baseUrl;
        public DotPayService(IOptions<DotPayOptions> _dotPay, IConfig config, IHttpContextAccessor httpContext, IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            dotPayOptions = _dotPay.Value;
            this.config = config;
            this.httpContext = httpContext.HttpContext;
            baseUrl = $"{this.httpContext.Request.Scheme}://{this.httpContext.Request.Host}/";
        }

        public async Task<string> GetLinkToPayment(Order order)
        {
            DotPayCreatePaymentRequest request = await GetRequestDto(order);
            request.Chk = await CalculateChk(request);
            string url = dotPayOptions.BaseUrl;
            var properties = request.GetType().GetProperties().Where(p => p.GetValue(request) != null)
                .Select(p => p.Name.ToLower() + "=" + HttpUtility.UrlEncode(p.GetValue(request).ToString()));
            string queryString = String.Join("&", properties.ToArray());

            return url + "/?" + queryString;
        }


        private async Task<string> CalculateChk(DotPayCreatePaymentRequest request)
        {
            var properties = request.GetType().GetProperties().Where(p => p.Name != "Chk")
                .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(request).ToString());
            var paramsList = "";
            SortedDictionary<string, string> sortedProps = new();
            foreach (var key in properties.Keys)
            {
                sortedProps.Add(key, properties[key]);
            }
            foreach (var key in sortedProps.Keys)
            {
                paramsList = paramsList + $"{key};";
            }
            paramsList = paramsList.Remove(paramsList.Length - 1);
            sortedProps.Add("paramsList", paramsList);
            string json = JsonConvert.SerializeObject(sortedProps, Formatting.None, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            var pin = await config.GetValue(ConfigKeys.DotPayKeys.Pin, repositoryWrapper.ConfigRepository);
            using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(pin));
            var hashedBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(json));
            var builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private async Task<DotPayCreatePaymentRequest> GetRequestDto(Order order)
        {
            return new DotPayCreatePaymentRequest
            {
                Id = int.Parse(await config.GetValue(ConfigKeys.DotPayKeys.Id, repositoryWrapper.ConfigRepository)),
                Amount = Math.Round(GetOrderAmmount(order), 2).ToString().Replace(',', '.'),
                Description = $"Zapłata za zamówienie nr {order.Id}",
                Url = this.baseUrl + "Order/PaymentConfirm",
                UrlC = this.baseUrl + "Order/PaymentStatus",
                Control = order.Id.ToString(),
                Email = order.Email == null ? httpContext.User.GetEmail() : order.Email
            };
        }
    }
}
