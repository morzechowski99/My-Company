//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.Extensions;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class UserAddressesViewComponent : ViewComponent
    {
        private IRepositoryWrapper repositoryWrapper;
        private HttpContext httpContext;

        public UserAddressesViewComponent(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Address> addresses = null;
            if (User.Identity.IsAuthenticated)
            {
                var name = httpContext.User.GetId();
                addresses = await repositoryWrapper.AddressesRepository.GetAddressesByUser(name);
            }
            else
            {
                addresses = new();
            }

            return View("UserAddresses", addresses);
        }
    }
}