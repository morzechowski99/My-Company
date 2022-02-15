//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IOrderPackingRepository : IRepositoryBase<Packing>
    {
        Task<List<ChartItem>> GetDataToChart(ChartEnums.ChartRange range);
    }
}
