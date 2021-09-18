using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Authorize(Roles = Constants.Roles.MainAdministrator)]
    [Area("Warehouse")]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public EmployeeController(IMapper mapper, IUsersService usersService)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Roles"] = getRolesListWithIds(await _usersService.GetWarehouseRoles());
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(getRolesList(), "Role", "RolePL");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = _mapper.Map<AppUser>(userViewModel);

                await _usersService.CreateUser(newUser, userViewModel.Role);

                return RedirectToAction(nameof(Index));
            }
            ViewData["Roles"] = new SelectList(getRolesList(), "Role", "RolePL");
            return View();
        }

        [HttpGet]
        public IActionResult GetList(EmployeeListFilters filters)
        {
            if (filters == null)
                return BadRequest();

            return ViewComponent("EmployeeList", filters);
        }

        [HttpPut]
        public async Task<IActionResult> LockUser(string userId)
        {
            try
            {
                if (userId == null)
                    return BadRequest("invalid request");

                AppUser user = await _usersService.GetUserById(userId);

                if (user == null)
                    return NotFound("no user with given userId");

                await _usersService.LockUser(user);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }

        }

        [HttpPut]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            try
            {
                if (userId == null)
                    return BadRequest("invalid request");

                AppUser user = await _usersService.GetUserById(userId);

                if (user == null)
                    return NotFound("no user with given userId");

                await _usersService.UnlockUser(user);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return BadRequest();

            var user = await _usersService.GetUserWithRolesById(id);

            if (user == null)
                return NotFound();

            var userDto = _mapper.Map<EditEmployeeViewModel>(user);

            var rolesList = getRolesList();
            ViewData["Roles"] = new SelectList(
                rolesList,
                "Role",
                "RolePL",
                rolesList.Find(role => role.Role == user.UserRoles.First().Role.Name).Role
            ); 

            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditEmployeeViewModel editEmployeeDto)
        {
            AppUser user = null;
            List<RoleViewModel> rolesList = null;
            if (ModelState.IsValid)
            {
                if (id != editEmployeeDto.Id)
                    return BadRequest();

                user = await _usersService.GetUserWithRolesById(id);

                if (user == null)
                    return NotFound();

                if (user.Email != editEmployeeDto.Email)
                    if (await _usersService.CheckEmail(editEmployeeDto.Email))
                    {
                        ModelState.AddModelError("Email", "Podany adres e-mail jest zajęty");
                        rolesList = getRolesList();
                        ViewData["Roles"] = new SelectList(
                            rolesList,
                            "Role",
                            "RolePL",
                            editEmployeeDto.Role
                        );
                        return View(editEmployeeDto);
                    }

                string prevName = user.Name;
                string prevSurname = user.Surname;

                var editedUser = _mapper.Map(editEmployeeDto, user);

                await _usersService.EditUser(editedUser, editEmployeeDto.Role, prevName, prevSurname);

                return RedirectToAction(nameof(Index));
            }
            rolesList = getRolesList();
            ViewData["Roles"] = new SelectList(
                rolesList,
                "Role",
                "RolePL",
                editEmployeeDto.Role
            );
            return View(editEmployeeDto);
        }
        #region Private
        private List<RoleViewModel> getRolesList()
        {
            return new List<RoleViewModel>() {
                new RoleViewModel { Role = Constants.Roles.MainAdministrator, RolePL = "Administartor" },
                new RoleViewModel { Role =  Constants.Roles.WarehouseEmployee,RolePL ="Pracownik" }
            };
        }

        private List<RoleViewModel> getRolesListWithIds(IEnumerable<AppRole> rolesDb)
        {
            List<RoleViewModel> roles = new();

            foreach (var role in rolesDb)
            {
                roles.Add(
                    new RoleViewModel
                    {
                        Id = role.Id,
                        RolePL = role.Name == Constants.Roles.MainAdministrator ? "Administartor" : "Pracownik"
                    }
                    );
            }
            return roles;
        }

        #endregion
    }
}
