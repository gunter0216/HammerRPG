namespace App.Common.Logger.Runtime
{
    public interface ILogger
    {
        void LogError(object value);
        void Log(object value);
    }
}