using AutoMapper;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.AutoMapper
{
    public class ToDTOProfile : Profile
    {
        public ToDTOProfile()
        {
            CreateMap<WarehouseRow, WarehouseRowViewModel>();

            CreateMap<WarehouseSector, WarehouseSectorViewModel>();
        }
    }
}
