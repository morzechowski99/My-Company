//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class EmployeeListViewComponent : ViewComponent
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public EmployeeListViewComponent(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(EmployeeListFilters filters)
        {
            var employees = _usersService.GetEmployeesByFilters(filters);

            var list = await PagedList<AppUser>
                .ToPagedList(employees, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 25);

            var listView = _mapper.Map<List<EmployeeListItem>>(list);

            return View("EmployeeList", new PagedList<EmployeeListItem>(listView, employees.Count(), list.CurrentPage, list.PageSize));
        }
    }

}
