using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Sprite.Runtime
{
    public class SpriteModuleConfig : ModuleConfig
    {
        [SerializeField] private UnityEngine.Sprite _sprite;

        public UnityEngine.Sprite Sprite => _sprite;

        public SpriteModuleConfig(UnityEngine.Sprite sprite)
        {
            _sprite = sprite;
        }
    }
}