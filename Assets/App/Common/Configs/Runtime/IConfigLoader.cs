using App.Common.Configs.External;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.Configs.Runtime
{
    public interface IConfigLoader
    {
        // T LoadConfig<T>(string localKey, string serverKey) where T : class;
        // Optional<T> LoadConfigFromServer<T>(string serverKey) where T : class;
        Optional<T> LoadConfig<T>(string localKey) where T : class;
        Optional<T> LoadGameConfig<T>(string localKey) where T : GameConfig;
    }
}