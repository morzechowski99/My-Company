namespace My_Company.Helpers
{
    public static class ProductsHelpers
    {
        public static decimal GetGrossPrice(int priceInPennies, int vat)
        {
            decimal value = priceInPennies / 100.0M;
            return decimal.Round(value + value * (vat / 100.0M), 2);
        }
    }
}
