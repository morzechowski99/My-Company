//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using My_Company.Helpers;
using System.Net;

namespace My_Company.Filters
{
    public class DotPayIpFilter : ActionFilterAttribute
    {
        private readonly string safelist;

        public DotPayIpFilter(IConfiguration configuration)
        {
            safelist = configuration.GetValue<string>(Constants.DOT_PAY_IPS);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            var ip = safelist.Split(';');
            var badIp = true;

            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            foreach (var address in ip)
            {
                var testIp = IPAddress.Parse(address);

                if (testIp.Equals(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
