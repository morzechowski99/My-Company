//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Hangfire.Dashboard;
using My_Company.Helpers;

namespace My_Company.Filters
{
    public class HangFireFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole(Constants.Roles.MainAdministrator);
        }
    }
}
