using UnityEngine;
using ILogger = App.Common.Logger.Runtime.ILogger;

namespace App.Common.Logger.External
{
    public class UnityLogger : ILogger
    {
        public void LogError(object value)
        {
            Debug.LogError(value);
        }

        public void Log(object value)
        {
            Debug.Log(value);
        }
    }
}