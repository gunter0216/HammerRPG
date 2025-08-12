using System.Collections;

namespace App.Common.Utilities.Utility.Runtime.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count <= 0;
        }
    }
}