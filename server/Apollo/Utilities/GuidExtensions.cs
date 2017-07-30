using System;

namespace Apollo.Utilities
{
    public static class GuidExtensions
    {
        public static string ToNakedString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}
