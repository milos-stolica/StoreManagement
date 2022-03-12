namespace StoreManagement.API.Common.Extensions
{
    public static class FloatExt
    {
        public static bool IsBetween(this float num, float min, float max)
        {
            return num >= min && num <= max;
        }
    }
}
