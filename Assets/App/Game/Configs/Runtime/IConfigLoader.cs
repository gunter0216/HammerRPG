using App.Common.Utility.Runtime;

namespace App.Game.Configs.Runtime
{
    public interface IConfigLoader
    {
        // T LoadConfig<T>(string localKey, string serverKey) where T : class;
        // Optional<T> LoadConfigFromServer<T>(string serverKey) where T : class;
        Optional<T> LoadLocalConfig<T>(string localKey) where T : class;
    }
}