using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime.Context;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.Runtime
{
    public class ContextInstanceAssetLoader : IContextInstanceAssetLoader
    {
        private readonly Dictionary<Type, Dictionary<Object, IKeyEvaluator>> _contextItems = new();

        private readonly IInstanceAssetLoader _instanceAssetLoader;
        
        public ContextInstanceAssetLoader(IInstanceAssetLoader instanceAssetLoader)
        {
            _instanceAssetLoader = instanceAssetLoader;
        }
        
        public Optional<T> InstantiateSync<T>(IKeyEvaluator key, Transform parent = null, Type context = null) where T : Object
        {
            if (context == null)
            {
                context = typeof(SceneAssetContext);
            }
            
            var instance = _instanceAssetLoader.InstantiateSync<T>(key, parent);
            if (!instance.HasValue)
            {
                return instance;
            }

            SaveInstanceContext(key, instance.Value, context);
            
            return instance;
        }

        public void UnloadContext(Type context)
        {
            if (!_contextItems.TryGetValue(context, out var contextDictionary))
            {
                // HLogger.LogError($"Context not found = {context}");
                return;
            }

            foreach (var keyValue in contextDictionary)
            {
                UnloadInstance(keyValue.Value, keyValue.Key);
            }
            
            contextDictionary.Clear();
        }

        private void SaveInstanceContext<T>(IKeyEvaluator key, T instance, Type context) where T : Object
        {
            if (!_contextItems.TryGetValue(context, out var contextDictionary))
            {
                contextDictionary = new Dictionary<Object, IKeyEvaluator>(1);
                _contextItems.Add(context, contextDictionary);
            }
            
            contextDictionary.Add(instance, key);
        }

        public void UnloadInstance(IKeyEvaluator key, Object item)
        {
            _instanceAssetLoader.UnloadInstance(key, item);
        }

        public Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object
        {
            return _instanceAssetLoader.LoadSync<T>(key);
        }

        public void UnloadAsset(IKeyEvaluator key)
        {
            _instanceAssetLoader.UnloadAsset(key);
        }

        public void UnloadUnusedAssets()
        {
            _instanceAssetLoader.UnloadUnusedAssets();
        }

        public void MarkToUnload(IKeyEvaluator key)
        {
            _instanceAssetLoader.MarkToUnload(key);
        }
    }
}