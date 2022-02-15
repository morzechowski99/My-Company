//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IPickingRepository : IRepositoryBase<Picking>
    {
        Task<List<PickingItem>> GetItems(Guid pickingId);
        Task<List<ChartItem>> GetDataToChart(ChartEnums.ChartRange range);
    }
}
