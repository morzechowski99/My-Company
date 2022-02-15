//Program powstał na Wydziale Informatyki Politechniki Białostockiej
namespace My_Company.Areas.Warehouse.EnumTypes
{
    public static class ChartEnums
    {
        public enum ChartMode
        {
            Orders = 0,
            Completions = 1,
            Packing = 2
        }

        public enum ChartRange
        {
            Week = 0,
            Month = 1,
            Year = 2
        }
    }
}
