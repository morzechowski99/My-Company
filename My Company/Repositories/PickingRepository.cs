using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class PickingRepository : RepositoryBase<Picking>, IPickingRepository
    {
        public PickingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ChartItem>> GetDataToChart(ChartEnums.ChartRange range)
        {
            var now = DateTime.Now.Date;
            List<ChartItem> chartItems = new();
            switch (range)
            {
                case ChartEnums.ChartRange.Week:
                    {
                        DateTime firstDay = now.AddDays(-4);
                        IEnumerable<IGrouping<DayOfWeek, Picking>> items = (await FindByCondition(p => p.End >= firstDay).ToListAsync()).Where(p => p.End.HasValue).GroupBy(p => p.End.Value.DayOfWeek);
                        for (int i = 0; i < 5; i++)
                        {
                            var dayOfWeek = (int)(firstDay.DayOfWeek + i) % 7;
                            var item = items.FirstOrDefault(i => (int)i.Key == dayOfWeek);
                            chartItems.Add(new ChartItem { Index = dayOfWeek, Value = item == null ? 0 : item.Count() });
                        }
                        break;
                    }
                case ChartEnums.ChartRange.Month:
                    {
                        DateTime firstMonth = now.AddMonths(-4);
                        IEnumerable<IGrouping<int, Picking>> items = (await FindByCondition(p => p.End >= firstMonth).ToListAsync()).Where(p => p.End.HasValue).GroupBy(p => p.End.Value.Month);
                        for (int i = 0; i < 5; i++)
                        {
                            var month = (((int)(firstMonth.Month + i) - 1) % 12) + 1;
                            var item = items.FirstOrDefault(i => (int)i.Key == month);
                            chartItems.Add(new ChartItem { Index = month, Value = item == null ? 0 : item.Count() });
                        }
                        break;
                    }
                case ChartEnums.ChartRange.Year:
                    {
                        DateTime firstYear = now.AddYears(-4);
                        IEnumerable<IGrouping<int, Picking>> items = (await FindByCondition(p => p.End >= firstYear).ToListAsync()).Where(p => p.End.HasValue).GroupBy(p => p.End.Value.Year);
                        for (int i = 0; i < 5; i++)
                        {
                            var year = (int)firstYear.Year + i;
                            var item = items.FirstOrDefault(i => (int)i.Key == year);
                            chartItems.Add(new ChartItem { Index = year, Value = item == null ? 0 : item.Count() });
                        }
                        break;
                    }
            }
            return chartItems;
        }

        public async Task<List<PickingItem>> GetItems(Guid pickingId)
        {
            var picking = await FindByCondition(p => p.OrderId == pickingId)
                .Include(p => p.PickingItems)
                .ThenInclude(pi => pi.Sector.Row)
                .FirstOrDefaultAsync();
            if (picking == null)
            {
                return null;
            }

            return picking.PickingItems.ToList();
        }
    }
}
