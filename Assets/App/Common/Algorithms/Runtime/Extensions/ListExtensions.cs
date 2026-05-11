using System;
using System.Collections.Generic;

namespace App.Common.Algorithms.Runtime.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random _random = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = _random.Next(0, i + 1);

                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}