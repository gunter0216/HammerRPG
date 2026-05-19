using System;

namespace App.Common.Algorithms.Runtime.Extensions
{
    public static class ArrayExtensions
    {
        private static readonly Random _random = new();

        public static void Shuffle<T>(this T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = _random.Next(0, i + 1);

                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }
        
        public static void Swap<T>(this T[] array, int first, int second)
        {
            (array[first], array[second]) = (array[second], array[first]);
        }
    }
}