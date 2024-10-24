﻿using System.Collections;
using System.Collections.Generic;

namespace App.Common.Utility.Runtime.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count <= 0;
        }
    }
}