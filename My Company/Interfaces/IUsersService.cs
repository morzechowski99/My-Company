//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Identity;
using My_Company.Areas.Shop.ViewModels.Login;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using My_Company.Models.AccountModels;
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
        Task EditUser(AppUser editedUser, string role, string prevName, string prevSurname);
        Task<bool> CheckEmail(string email);
        Task<IdentityResult> CreateShopUser(RegisterModel user);
        Task<VerifyUserData> GenerateEmailConfirmationData(string emailRegister);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<IdentityResult> ChangePassword(AppUser user, string oldPassword, string newPassword);
        Task UpdateUser(AppUser user);
    }
}
