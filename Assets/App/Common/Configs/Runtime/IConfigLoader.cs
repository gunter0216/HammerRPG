using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Configs.Runtime
{
    public interface IConfigLoader
    {
        // T LoadConfig<T>(string localKey, string serverKey) where T : class;
        // Optional<T> LoadConfigFromServer<T>(string serverKey) where T : class;
        Optional<T> LoadConfig<T>(string localKey) where T : class;
    }
}