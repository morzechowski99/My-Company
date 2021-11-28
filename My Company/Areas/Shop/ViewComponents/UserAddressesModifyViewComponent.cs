using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.Extensions;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class UserAddressesModifyViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly HttpContext httpContext;
        public UserAddressesModifyViewComponent(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repositoryWrapper = repositoryWrapper;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var addresses = await repositoryWrapper.AddressesRepository.GetAddressesByUser(httpContext.User.GetId());
            return View("UserAddressesModify", addresses);
        }
    }
}
