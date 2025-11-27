using System.Runtime.CompilerServices;
using UnityEngine;
using ILogger = App.Common.Logger.Runtime.ILogger;

namespace App.Common.Logger.External
{
    public class UnityLogger : ILogger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(object value)
        {
            Debug.LogError(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(object value)
        {
            Debug.Log(value);
        }
    }
}