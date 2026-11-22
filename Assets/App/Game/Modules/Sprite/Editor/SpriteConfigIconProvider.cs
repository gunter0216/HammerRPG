using App.Common.Configs.External;
using App.Common.ModuleItem.Runtime.Config;
using App.Game.Modules.Sprite.Runtime;
using Game.Core.Modules.Config.Editor.ScriptableWindows;
using UnityEditor;

namespace App.Game.Modules.Sprite.Editor
{
    internal static class SpriteConfigIconProvider
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            ConfigWindow.ConfigIconProvider = GetIcon;
        }

        private static UnityEngine.Sprite GetIcon(GameConfig config)
        {
            return config is ModuleItemGameConfig item && item.Modules != null &&
                   item.TryGetModule<SpriteModuleConfig>(out var module)
                ? module.Sprite
                : null;
        }
    }
}
