using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime.Deserializer;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Game.Configs.External
{
    [Singleton]
    public class ConfigLoader : IConfigLoader
    {
        public const string PlayerPrefsUseLocalConfigsKey = "TestingPanel_UseLocalConfigs";

        [Inject] private readonly IAssetManager m_AssetManager;
        [Inject] private readonly IJsonDeserializer m_JsonDeserializer;

        private bool UseLocalConfigs => PlayerPrefs.GetInt(PlayerPrefsUseLocalConfigsKey, 0) == 1;


        public Optional<T> LoadConfig<T>(string localKey) where T : class
        {
            var configKeyEvaluator = new StringKeyEvaluator(localKey);
            var configJsonResult = m_AssetManager.LoadSync<TextAsset>(configKeyEvaluator);
            
            if (configJsonResult.HasValue)
            {
                var configResult = m_JsonDeserializer.Deserialize<T>(configJsonResult.Value.text);
                m_AssetManager.UnloadAsset(configKeyEvaluator);
                return configResult;
            }
            
            Debug.LogError($"[ConfigLoader] In method LoadLocalConfig, cant load local config {typeof(T).Name} with key {localKey}.");
            
            return Optional<T>.Empty;
        }
        
        // public T LoadConfig<T>(string localKey, string serverKey) where T : class
        // {
        //     Result<T> localConfigResult;
        //     Result<T> serverConfigResult;
        //     if (UseLocalConfigs)
        //     {
        //         localConfigResult = LoadLocalConfig<T>(localKey);
        //         if (localConfigResult.IsExist) 
        //         {
        //             return localConfigResult.Object;
        //         }
        //         
        //         serverConfigResult = LoadConfigFromServer<T>(serverKey);
        //         if (serverConfigResult.IsExist)
        //         {
        //             return serverConfigResult.Object;
        //         }
        //
        //         return default;
        //     }
        //
        //     if (serverKey.IsNullOrEmpty())
        //     {
        //         Debug.LogError($"[ConfigLoader] In method LoadConfig, server key is null.");
        //         localConfigResult = LoadLocalConfig<T>(localKey);
        //         if (localConfigResult.IsExist) 
        //         {
        //             return localConfigResult.Object;
        //         }
        //
        //         return default;
        //     }
        //
        //     serverConfigResult = LoadConfigFromServer<T>(serverKey);
        //     if (serverConfigResult.IsExist)
        //     {
        //         return serverConfigResult.Object;
        //     }
        //
        //     localConfigResult = LoadLocalConfig<T>(localKey);
        //     if (localConfigResult.IsExist) 
        //     {
        //         return localConfigResult.Object;
        //     }
        //
        //     return default;
        // }
        //
        // public Result<T> LoadConfigFromServer<T>(string serverKey) where T : class
        // {
        //     if (UseLocalConfigs)
        //     {
        //         return new Result<T>(default, false);
        //     }
        //     
        //     var result =  m_EpicLiveOpsContainer.Get<T>(serverKey);
        //     if (!result.IsExist)
        //     {
        //         Debug.LogWarning($"[ConfigLoader] In method LoadConfigFromServer, cant load config {typeof(T).Name} by key {serverKey}, error {result.Reason}.");
        //         return new Result<T>(default, false);
        //     }
        //
        //     return new Result<T>(result.Object, true);
        // }
    }
}