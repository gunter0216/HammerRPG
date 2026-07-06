using System.Collections.Generic;
using App.Common.Configs.External;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Configs.Runtime
{
    public interface IConfigLoader
    {
        Optional<T> LoadConfig<T>(string localKey) where T : class;
        Optional<T> LoadGameConfig<T>(string localKey) where T : GameConfig;
        Optional<List<T>> LoadGameConfigs<T>(string tag) where T : GameConfig;
    }
}