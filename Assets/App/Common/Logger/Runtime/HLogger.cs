namespace App.Common.Logger.Runtime
{
    public class HLogger : ILogger
    {
        private static ILogger m_Instance;

        public static void SetInstance(ILogger logger)
        {
            m_Instance = logger;
        }
        
        public static void LogError(object value)
        {
            m_Instance.LogError(value);
        }
        
        public static void Log(object value)
        {
            m_Instance.Log(value);
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