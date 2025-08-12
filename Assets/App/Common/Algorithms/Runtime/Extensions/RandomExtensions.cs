using System;

namespace Assets.App.Common.Algorithms.Runtime.Extensions
{
    public static class RandomExtensions
    {
        public static Vector2 RandomInUnitCircle(this Random random)
        {
            double angle = random.NextDouble() * 2 * Math.PI;
            double radius = Math.Sqrt(random.NextDouble());

            float x = (float)(Math.Cos(angle) * radius);
            float y = (float)(Math.Sin(angle) * radius);

            return new Vector2(x, y);
        }
    }
}