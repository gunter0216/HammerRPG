#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Utils.Common
{
    public static class EditorUtils
    {
        private class CachedAssetData
        {
            public string[] Guids;
            public string[] Paths;
            public Object[] Assets;
            public Dictionary<string, Object> AssetsById;
            public int LastAssetChangeCount;
        }

        private static readonly Dictionary<Type, CachedAssetData> _assetCache = new();
        private static int _projectChangeCounter;

        static EditorUtils()
        {
            EditorApplication.projectChanged += OnProjectChanged;
        }

        private static void OnProjectChanged()
        {
            _assetCache.Clear();
            _projectChangeCounter++;
        }

        public static void SetDirtyIfNot(Object obj)
        {
            if (!EditorUtility.IsDirty(obj))
                EditorUtility.SetDirty(obj);
        }

        public static void SaveSerialization(Object obj)
        {
            EditorUtility.SetDirty(obj);
            SaveAssets();
        }

        public static void SaveSerialization(SerializedObject serializedObject)
        {
            serializedObject.ApplyModifiedProperties();
            SaveAssets();
        }

        public static void SaveSerialization(SerializedProperty serializedProperty) 
            => SaveSerialization(serializedProperty.serializedObject);

        public static void SaveAssets() 
            => SaveAssetsAsync().Forget();

        private static CachedAssetData GetCachedAssets(Type type)
        {
            if (!_assetCache.TryGetValue(type, out var cachedData))
            {
                cachedData = new CachedAssetData
                {
                    LastAssetChangeCount = -1
                };
                _assetCache[type] = cachedData;
            }

            if (cachedData.LastAssetChangeCount != _projectChangeCounter || cachedData.Guids == null)
            {
                cachedData.Guids = AssetDatabase.FindAssets($"t:{type.Name}");
                cachedData.Paths = new string[cachedData.Guids.Length];
                cachedData.Assets = new Object[cachedData.Guids.Length];
                
                for (int i = 0; i < cachedData.Guids.Length; i++)
                {
                    cachedData.Paths[i] = AssetDatabase.GUIDToAssetPath(cachedData.Guids[i]);
                    cachedData.Assets[i] = AssetDatabase.LoadAssetAtPath(cachedData.Paths[i], type);
                }
                
                cachedData.AssetsById = null;
                cachedData.LastAssetChangeCount = _projectChangeCounter;
            }

            return cachedData;
        }

        public static T[] GetAssets<T>() where T : Object
        {
            CachedAssetData cached = GetCachedAssets(typeof(T));
            return cached.Assets.OfType<T>().ToArray();
        }

        public static Object[] GetAssets(Type type)
        {
            CachedAssetData cached = GetCachedAssets(type);
            return cached.Assets;
        }

        public static TCastTo[] GetAssets<TCastTo>(Type type)
        {
            CachedAssetData cached = GetCachedAssets(type);
            return cached.Assets.Cast<TCastTo>().ToArray();
        }

        public static T[] GetAssets<T>(string folder) where T : Object
        {
            string[] assetsGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder });
            var assets = new T[assetsGuids.Length];
            for (var i = 0; i < assetsGuids.Length; i++)
            {
               string assetPath = AssetDatabase.GUIDToAssetPath(assetsGuids[i]);
               assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            
            return assets;
        }

        public static void OpenFolder(string folderPath)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(folderPath, typeof(Object));
            EditorUtility.FocusProjectWindow();
            var projectBrowser = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
            
            object lastInteractedBrowser = projectBrowser
                !.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public)
                ?.GetValue(null);
            
            MethodInfo showDirectoryMethod = projectBrowser
                .GetMethod("ShowFolderContents", BindingFlags.NonPublic | BindingFlags.Instance);
            
            showDirectoryMethod!.Invoke(lastInteractedBrowser, new object[ ] { obj.GetInstanceID(), true });
        }

        public static HashSet<Object> GetActiveSelections(Type assetType)
            => Selection.assetGUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, assetType))
                .ToHashSet();

        private static async UniTask SaveAssetsAsync()
        {
            // Avoiding unity draw editor exceptions
            await UniTask.WaitForSeconds(0.05f);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif