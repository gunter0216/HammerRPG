// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Game.Core.Utils.Common;
// using UnityEditor;
// using UnityEngine;
//
// namespace Game.Core.Modules.Config.Editor
// {
//     [CustomPropertyDrawer(typeof(ConfigSelector<>), true)]
//     public class ConfigSelectorDrawer : PropertyDrawer
//     {
//         private readonly Color _errorButtonColor = new(1f, 0, 0, 0.7f);
//         private readonly Color _errorTextColor = new(1f, 0, 0, 1f);
//         private const float VerticalPadding = 5f;
//
//         private class ConfigTypeCache
//         {
//             public IMultipleConfig[] Configs;
//             public Dictionary<string, ScriptableObject> ConfigsById;
//             public int LastAssetChangeCount;
//         }
//
//         private class PropertyCache
//         {
//             public ScriptableObject FoundConfig;
//             public string LastIdValue;
//         }
//
//         private static readonly Dictionary<Type, ConfigTypeCache> _configsByType = new();
//         private static readonly Dictionary<string, PropertyCache> _propertyCache = new();
//         private static readonly Dictionary<string, Type> _typeCache = new();
//         private static readonly Dictionary<Type, CachedAssetData> _assetCache = new();
//         private static int _projectChangeCounter;
//
//         private class CachedAssetData
//         {
//             public string[] Guids;
//             public string[] Paths;
//             public int LastAssetChangeCount;
//         }
//
//         static ConfigSelectorDrawer()
//         {
//             EditorApplication.projectChanged += OnProjectChanged;
//         }
//
//         private static void OnProjectChanged()
//         {
//             _configsByType.Clear();
//             _propertyCache.Clear();
//             _typeCache.Clear();
//             _assetCache.Clear();
//             _projectChangeCounter++;
//         }
//
//         private static CachedAssetData GetCachedAssets(Type configType)
//         {
//             if (!_assetCache.TryGetValue(configType, out var cachedData))
//             {
//                 cachedData = new CachedAssetData
//                 {
//                     LastAssetChangeCount = -1
//                 };
//                 _assetCache[configType] = cachedData;
//             }
//
//             if (cachedData.LastAssetChangeCount != _projectChangeCounter || cachedData.Guids == null)
//             {
//                 cachedData.Guids = AssetDatabase.FindAssets($"t:{configType.Name}");
//                 cachedData.Paths = new string[cachedData.Guids.Length];
//                 for (int i = 0; i < cachedData.Guids.Length; i++)
//                 {
//                     cachedData.Paths[i] = AssetDatabase.GUIDToAssetPath(cachedData.Guids[i]);
//                 }
//                 cachedData.LastAssetChangeCount = _projectChangeCounter;
//             }
//
//             return cachedData;
//         }
//
//         private static ConfigTypeCache GetConfigsByType(Type configType)
//         {
//             if (!_configsByType.TryGetValue(configType, out var typeCache))
//             {
//                 typeCache = new ConfigTypeCache
//                 {
//                     LastAssetChangeCount = -1
//                 };
//                 _configsByType[configType] = typeCache;
//             }
//
//             if (typeCache.LastAssetChangeCount != _projectChangeCounter || typeCache.Configs == null)
//             {
//                 CachedAssetData assetData = GetCachedAssets(configType);
//                 var configs = new List<IMultipleConfig>(assetData.Paths.Length);
//                 
//                 foreach (string path in assetData.Paths)
//                 {
//                     var asset = AssetDatabase.LoadAssetAtPath(path, configType);
//                     if (asset is IMultipleConfig config)
//                     {
//                         configs.Add(config);
//                     }
//                 }
//                 
//                 typeCache.Configs = configs.ToArray();
//                 typeCache.ConfigsById = new Dictionary<string, ScriptableObject>(typeCache.Configs.Length);
//                 foreach (var config in typeCache.Configs)
//                 {
//                     if (config is ScriptableObject so && !string.IsNullOrEmpty(config.Id))
//                     {
//                         typeCache.ConfigsById[config.Id] = so;
//                     }
//                 }
//                 typeCache.LastAssetChangeCount = _projectChangeCounter;
//             }
//
//             return typeCache;
//         }
//
//         private PropertyCache GetPropertyCache(SerializedProperty property)
//         {
//             int instanceId = property.serializedObject.targetObject.GetInstanceID();
//             string key = $"{instanceId}.{property.propertyPath}";
//             
//             if (!_propertyCache.TryGetValue(key, out var propCache))
//             {
//                 propCache = new PropertyCache();
//                 _propertyCache[key] = propCache;
//             }
//
//             return propCache;
//         }
//
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             Type configType = GetConfigType(property);
//             var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth,
//                 EditorGUIUtility.singleLineHeight);
//             Rect dropdownRect = GetDropdownRect(position, property);
//
//             if (!typeof(IMultipleConfig).IsAssignableFrom(configType) ||
//                 !typeof(ScriptableObject).IsAssignableFrom(configType))
//             {
//                 DrawErrorLabel(labelRect, dropdownRect, label, $"Config must be {nameof(ScriptableObject)}");
//                 return;
//             }
//
//             if (configType == typeof(IMultipleConfig) || configType == typeof(MultipleConfig))
//             {
//                 DrawErrorLabel(labelRect, dropdownRect, label, "Select concrete config type");
//                 return;
//             }
//
//             ConfigTypeCache typeCache = GetConfigsByType(configType);
//             if (typeCache.Configs == null || typeCache.Configs.Length == 0)
//             {
//                 DrawErrorLabel(labelRect, dropdownRect, label, "No configs found");
//                 return;
//             }
//
//             SerializedProperty idProperty = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Id)}>k__BackingField");
//             SerializedProperty nameProperty = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Name)}>k__BackingField");
//             
//             PropertyCache propCache = GetPropertyCache(property);
//             string currentId = idProperty?.stringValue ?? string.Empty;
//             if (propCache.LastIdValue != currentId || propCache.FoundConfig == null)
//             {
//                 typeCache.ConfigsById.TryGetValue(currentId, out propCache.FoundConfig);
//                 propCache.LastIdValue = currentId;
//             }
//
//             DrawField(labelRect, dropdownRect, label, typeCache, propCache, idProperty, nameProperty);
//         }
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             float lineHeight = EditorGUIUtility.singleLineHeight;
//             Type configType = GetConfigType(property);
//             
//             if (!typeof(IMultipleConfig).IsAssignableFrom(configType) ||
//                 !typeof(ScriptableObject).IsAssignableFrom(configType))
//                 return lineHeight;
//
//             if (configType == typeof(IMultipleConfig) || configType == typeof(MultipleConfig))
//                 return lineHeight;
//
//             ConfigTypeCache typeCache = GetConfigsByType(configType);
//             if (typeCache.Configs == null || typeCache.Configs.Length == 0)
//                 return lineHeight;
//
//             SerializedProperty idProperty = property.FindPropertyRelative($"<{nameof(ConfigSelector<IMultipleConfig>.Id)}>k__BackingField");
//             PropertyCache propCache = GetPropertyCache(property);
//             string currentId = idProperty?.stringValue ?? string.Empty;
//             if (propCache.LastIdValue != currentId || propCache.FoundConfig == null)
//             {
//                 typeCache.ConfigsById.TryGetValue(currentId, out propCache.FoundConfig);
//                 propCache.LastIdValue = currentId;
//             }
//
//             if (propCache.FoundConfig != null)
//                 return (lineHeight + VerticalPadding) * 2;
//
//             return lineHeight;
//         }
//
//         private void DrawField(
//             Rect labelRect,
//             Rect dropdownRect,
//             GUIContent label,
//             ConfigTypeCache typeCache,
//             PropertyCache propCache,
//             SerializedProperty idProperty,
//             SerializedProperty nameProperty)
//         {
//             EditorGUI.PrefixLabel(labelRect, label);
//             bool dropdownButton;
//
//             if (propCache.FoundConfig != null)
//             {
//                 Rect pingButtonRect = ReservePingButtonRect(ref dropdownRect);
//                 
//                 dropdownButton =
//                     EditorGUI.DropdownButton(dropdownRect, new GUIContent(propCache.FoundConfig.name), FocusType.Keyboard);
//                 
//                 if (GUI.Button(pingButtonRect, "Ping"))
//                 {
//                     EditorGUIUtility.PingObject(propCache.FoundConfig);
//                 }
//             }
//             else if (!string.IsNullOrEmpty(nameProperty?.stringValue))
//             {
//                 Color previousColor = GUI.color;
//                 GUI.color = _errorButtonColor;
//                 dropdownButton = EditorGUI.DropdownButton(dropdownRect, new GUIContent(nameProperty.stringValue),
//                     FocusType.Keyboard);
//                 GUI.color = previousColor;
//             }
//             else
//             {
//                 dropdownButton = EditorGUI.DropdownButton(dropdownRect, new GUIContent("(None)"), FocusType.Keyboard);
//             }
//
//             if (dropdownButton)
//                 ShowConfigDropdown(typeCache, propCache, idProperty, nameProperty, dropdownRect);
//         }
//
//
//         private void DrawErrorLabel(Rect labelRect, Rect dropdownRect, GUIContent label, string dropdownText)
//         {
//             EditorGUI.PrefixLabel(labelRect, label);
//             Color previousColor = GUI.color;
//             GUI.color = _errorTextColor;
//             EditorGUI.LabelField(dropdownRect, dropdownText);
//             GUI.color = previousColor;
//         }
//
//         private void ShowConfigDropdown(ConfigTypeCache typeCache, PropertyCache propCache, SerializedProperty idProperty, SerializedProperty nameProperty, Rect dropdownRect)
//         {
//             string currentId = idProperty?.stringValue ?? string.Empty;
//             
//             var dropdownItems = new List<SimpleDropdownItem<string>>(typeCache.Configs.Length);
//             foreach (var config in typeCache.Configs)
//             {
//                 if (config is ScriptableObject so)
//                 {
//                     dropdownItems.Add(new SimpleDropdownItem<string>(
//                         value: config.Id,
//                         label: so.name,
//                         isSelected: config.Id == currentId));
//                 }
//             }
//             
//             SimpleDropdownPopup<string>.Show(dropdownItems, OnItemSelected, dropdownRect);
//             return;
//
//             void OnItemSelected(SimpleDropdownItem<string> selectedItem)
//                 => OnDropdownItemSelected(selectedItem.Value, typeCache, propCache, idProperty, nameProperty);
//         }
//
//         private void OnDropdownItemSelected(string selectedId, ConfigTypeCache typeCache, PropertyCache propCache, SerializedProperty idProperty, SerializedProperty nameProperty)
//         {
//             idProperty.stringValue = selectedId;
//             typeCache.ConfigsById.TryGetValue(selectedId, out propCache.FoundConfig);
//             nameProperty.stringValue = propCache.FoundConfig != null ? propCache.FoundConfig.name : null;
//             propCache.LastIdValue = selectedId;
//             EditorUtils.SaveSerialization(idProperty);
//         }
//
//         private Type GetConfigType(SerializedProperty property)
//         {
//             string typeKey = fieldInfo.FieldType.FullName ?? fieldInfo.FieldType.Name;
//             
//             if (!_typeCache.TryGetValue(typeKey, out Type configType))
//             {
//                 Type fieldType = fieldInfo.FieldType;
//                 
//                 Type enumerableField = fieldType
//                     .GetInterfaces()
//                     .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
//
//                 Type selectorType = enumerableField != null
//                     ? enumerableField.GetGenericArguments().FirstOrDefault()
//                     : fieldType;
//
//                 configType = selectorType?.GetGenericArguments().SingleOrDefault() ?? typeof(IMultipleConfig);
//                 _typeCache[typeKey] = configType;
//             }
//             
//             return configType;
//         }
//
//         private static Rect GetDropdownRect(Rect position, SerializedProperty property)
//             => new(position.x + EditorGUIUtility.labelWidth, position.y,
//                 position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
//
//         private static Rect ReservePingButtonRect(ref Rect fieldRect)
//         {
//             const int pingWidth = 48;
//             const int spacing = 4;
//             fieldRect.width -= pingWidth + spacing;
//             return new Rect(fieldRect.x + fieldRect.width + spacing, fieldRect.y, pingWidth, fieldRect.height);
//         }
//     }
// }