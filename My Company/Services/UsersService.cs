using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My_Company.Areas.Warehouse.EnumTypes;

namespace My_Company.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly RoleManager<AppRole> _roleManager;

        public UsersService(UserManager<AppUser> userManager, IEmailSender emailSender, IRepositoryWrapper repositoryWrapper, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _repositoryWrapper = repositoryWrapper;
            _roleManager = roleManager;
        }

        public async Task CreateUser(AppUser newUser, string role)
        {
            newUser.EmailConfirmed = true;
            newUser.Name = newUser.Name.Trim();
            newUser.Surname = newUser.Surname.Trim();
            newUser.UserName = await getUserName(newUser);

            string password = GeneratePassword();

            IdentityResult checkUser = await _userManager.CreateAsync(newUser, GeneratePassword());

            if (checkUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, role);
            }

            string message = $"Hasło dla konta {newUser.UserName} to {password}";

            await _emailSender.SendEmailAsync(newUser.Email, "Hasło do konta", message);
        }

        private async Task<string> getUserName(AppUser newUser)
        {
            int count = await _repositoryWrapper.UserRepository.GetUsersWithSameNameAndSurnameCount(newUser.Name, newUser.Surname);
            return $"{newUser.Name.ToLower()}_{newUser.Surname.ToLower()}" + (count == 0 ? "" : "_" + (count + 1).ToString());
        }

        private string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

        public IQueryable<AppUser> GetEmployees()
        {
            return _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(user => user.UserRoles
                    .Any(ur => ur.Role.Name == Constants.Roles.MainAdministrator || ur.Role.Name == Constants.Roles.WarehouseEmployee));
        }

        public IQueryable<AppUser> GetEmployeesByFilters(EmployeeListFilters filters)
        {

            var employees = GetEmployees();
            if (!string.IsNullOrEmpty(filters.SearchString))
            {
                var searchString = filters.SearchString.ToLower();
                employees = employees.Where(emp => emp.Name.ToLower().Contains(searchString)
               || searchString.Contains(emp.Name.ToLower())
               || emp.Surname.ToLower().Contains(searchString)
               || searchString.Contains(emp.Surname.ToLower())
               || emp.UserName.ToLower().Contains(searchString)
               || searchString.Contains(emp.UserName.ToLower())
               || emp.Email.ToLower().Contains(searchString)
               || searchString.Contains(emp.Email.ToLower()));
            }
            if (!string.IsNullOrEmpty(filters.RoleId))
                employees = employees.Where(emp => emp.UserRoles.Any(ur => ur.RoleId == filters.RoleId));

            switch (filters.SortOrder)
            {
                case EmployeeListSortOrderEnum.NameAndSurnameASC:
                    employees = employees.OrderBy(emp => emp.Surname).ThenBy(emp => emp.Name);
                    break;
                case EmployeeListSortOrderEnum.NameAndSurnameDESC:
                    employees = employees.OrderByDescending(emp => emp.Surname).ThenBy(emp => emp.Name);
                    break;
                case EmployeeListSortOrderEnum.EmailASC:
                    employees = employees.OrderBy(emp => emp.Email);
                    break;
                case EmployeeListSortOrderEnum.EmailDESC:
                    employees = employees.OrderByDescending(emp => emp.Email);
                    break;
                case EmployeeListSortOrderEnum.UserNameASC:
                    employees = employees.OrderBy(emp => emp.UserName);
                    break;
                case EmployeeListSortOrderEnum.UserNameDESC:
                    employees = employees.OrderByDescending(emp => emp.UserName);
                    break;

            }

            return employees;
        }

        public async Task<IEnumerable<AppRole>> GetWarehouseRoles()
        {
            return await _roleManager.Roles
                .Where(role => role.Name == Constants.Roles.MainAdministrator || role.Name == Constants.Roles.WarehouseEmployee)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task LockUser(AppUser user)
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(1000));
        }
        
        public async Task UnlockUser(AppUser user)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }

        public async Task<AppUser> GetUserWithRolesById(string id)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
