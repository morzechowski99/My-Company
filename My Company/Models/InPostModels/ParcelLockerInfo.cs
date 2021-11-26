namespace My_Company.Models.InPostModels
{
    public class ParcelLockerInfo
    {
        public string Name { get; set; }
        public ParcelLocekrAddress Address { get; set; }
    }

    public class ParcelLocekrAddress
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
    }
}
