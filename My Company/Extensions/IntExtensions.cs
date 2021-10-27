namespace My_Company.Extensions
{
    public static class IntExtensions
    {
        public static string ToBarcode(this int value)
        {
            return ("0000000000000" + value)[^13..];
        }
    }
}
