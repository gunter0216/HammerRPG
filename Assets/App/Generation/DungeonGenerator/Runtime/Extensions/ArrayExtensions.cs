namespace App.Generation.DungeonGenerator.Runtime.Extensions
{
    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = value;
            }
        }
    }
}