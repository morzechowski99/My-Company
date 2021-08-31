using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IUsersService
    {
        Task CreateUser(AppUser newUser, string role);
        IQueryable<AppUser> GetEmployees();
        IQueryable<AppUser> GetEmployeesByFilters(EmployeeListFilters filters);
        Task<IEnumerable<AppRole>> GetWarehouseRoles();
        Task<AppUser> GetUserById(string userId);
        Task LockUser(AppUser user);
        Task UnlockUser(AppUser user);
        Task<AppUser> GetUserWithRolesById(string id);
    }
}
