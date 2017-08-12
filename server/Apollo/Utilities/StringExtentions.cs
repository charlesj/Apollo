using System;

namespace Apollo.Utilities
{
    public static class StringExtentions
    {
        public static DateTime ToDateTime(this string str)
        {
            DateTime result;
            DateTime.TryParse(str, out result);
            return result;
        }
    }
}
