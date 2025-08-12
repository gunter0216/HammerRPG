namespace App.Common.Utilities.Utility.Runtime.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Length <= 0;
        }
    }
}