using System;
using System.Collections.Generic;

namespace App.Common.Utilities.Utility.Runtime
{
    public static class RandomHelper
    {
        private static readonly Random _random = new Random();

        public static T GetRandomFromList<T>(IList<T> list)
        {
            var index = _random.Next(list.Count);
            return list[index];
        }
    }
}