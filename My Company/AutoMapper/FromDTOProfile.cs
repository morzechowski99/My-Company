using AutoMapper;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.AutoMapper
{
    public class FromDTOProfile: Profile
    {
        public FromDTOProfile()
        {
            CreateMap<NewWarehouseViewModel, Warehouse>();
            CreateMap<NewProductViewModel, Product>();
            CreateMap<CreateUserViewModel, AppUser>();
        }
    }
}
