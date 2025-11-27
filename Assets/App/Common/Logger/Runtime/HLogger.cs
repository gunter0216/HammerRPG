using System.Runtime.CompilerServices;

namespace App.Common.Logger.Runtime
{
    public class HLogger : ILogger
    {
        private static ILogger m_Instance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetInstance(ILogger logger)
        {
            m_Instance = logger;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(object value)
        {
            m_Instance.LogError(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(object value)
        {
            m_Instance.Log(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ILogger.LogError(object value)
        {
            LogError(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ILogger.Log(object value)
        {
            Log(value);
        }
    }
}