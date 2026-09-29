using App.Common.ModuleItem.Runtime.Config;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace App.Common.ModuleItem.Editor
{
    public class ModuleItemAssetPostprocessor : AssetPostprocessor
    {
        // Вызывается после импорта/создания ассетов
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                // Проверяем, является ли ассет нашим ScriptableObject
                var asset = AssetDatabase.LoadMainAssetAtPath(assetPath) as ModuleItemGameConfig;
                if (asset == null) continue;

                // 1. Добавляем в Addressables
                var settings = AddressableAssetSettingsDefaultObject.Settings;
                if (settings != null)
                {
                    // Получаем GUID
                    string guid = AssetDatabase.AssetPathToGUID(assetPath);
                
                    // Получаем или создаем группу (например, "Default")
                    var group = settings.DefaultGroup; 
                
                    // Создаем или получаем entry
                    var entry = settings.CreateOrMoveEntry(guid, group, false, false);
                
                    // Можно задать адрес (например, по имени файла)
                    entry.address = $"ModuleItemGameConfig/{asset.name}";
                    
                    // ✅ Добавляем Addressables Label
                    // force: true — создаст метку, если её нет в проекте
                    entry.SetLabel("ModuleItemGameConfig", true, true);
                
                    // Сохраняем настройки
                    settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, entry, true);
                }
            }
        }
    }
}