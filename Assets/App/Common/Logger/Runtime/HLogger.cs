using UnityEngine;

namespace App.Common.Logger.Runtime
{
    public static class HLogger
    {
        public static void LogError(object value)
        {
            Debug.LogError(value);
        }

        public static void Log(object value)
        {
            Debug.Log(value);
        }
    }
}