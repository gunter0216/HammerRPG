using UnityEngine;

namespace App.Common.Logger.Runtime
{
    public class HLogger : ILogger
    {
        public static void LogError(object value)
        {
            Debug.LogError(value);
        }
        
        public static void Log(object value)
        {
            Debug.Log(value);
        }
        
        void ILogger.LogError(object value)
        {
            LogError(value);
        }

        void ILogger.Log(object value)
        {
            Log(value);
        }
    }
}