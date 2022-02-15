using Microsoft.AspNetCore.Http;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Middlewares
{
    public class ShopEnabledMiddleware
    {
        RequestDelegate _next;

        public ShopEnabledMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx, IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            if (!ctx.Request.Path.StartsWithSegments("/Warehouse") && !ctx.Request.Path.StartsWithSegments("/warehouse")&& !ctx.Request.Path.StartsWithSegments("/Identity"))
            {
                if (!await config.IsShopEnabled(repositoryWrapper.ConfigRepository))
                {
                    ctx.Response.StatusCode = 404;
                }
                else
                {
                    await _next(ctx);
                }
            }
            else
            {
                await _next(ctx);
            }
        }
    }
}
