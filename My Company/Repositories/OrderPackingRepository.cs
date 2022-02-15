//Program powstał na Wydziale Informatyki Politechniki Białostockiej
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
    public class OrderPackingRepository : RepositoryBase<Packing>, IOrderPackingRepository
    {
        public OrderPackingRepository(ApplicationDbContext context) : base(context)
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
                        IEnumerable<IGrouping<DayOfWeek, Packing>> items = (await FindByCondition(p => p.PackingEnd >= firstDay).ToListAsync()).Where(p => p.PackingEnd.HasValue).GroupBy(p => p.PackingEnd.Value.DayOfWeek);
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
                        IEnumerable<IGrouping<int, Packing>> items = (await FindByCondition(p => p.PackingEnd >= firstMonth).ToListAsync()).Where(p => p.PackingEnd.HasValue).GroupBy(p => p.PackingEnd.Value.Month);
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
                        IEnumerable<IGrouping<int, Packing>> items = (await FindByCondition(p => p.PackingEnd >= firstYear).ToListAsync()).Where(p => p.PackingEnd.HasValue).GroupBy(p => p.PackingEnd.Value.Year);
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
    }
}
