using System;

namespace StoreManagement.API.Utils
{
    public static class MathWrapper
    {
        public static float Round(float number, int decimals)
        {
            return (float)Math.Round(number, decimals);
        }
    }
}
