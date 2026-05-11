using System.Collections;
using System.Collections.Generic;

namespace App.Common.Utilities.Utility.Runtime.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count <= 0;
        }
        
        public static T Random<T>(this IList<T> list)
        {
            return RandomHelper.GetRandomFromList(list);
        }
    }
}