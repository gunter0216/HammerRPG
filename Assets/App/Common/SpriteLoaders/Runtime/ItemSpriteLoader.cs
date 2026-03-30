using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameTiles.External.Config.Model;
using UnityEngine;

namespace App.Common.SpriteLoaders.External
{
    public class ItemSpriteLoader : IItemSpriteLoader
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly ISpriteLoader _spriteLoader;

        public ItemSpriteLoader(IModuleItemsManager moduleItemsManager, ISpriteLoader spriteLoader)
        {
            _moduleItemsManager = moduleItemsManager;
            _spriteLoader = spriteLoader;
        }

        public Optional<Sprite> LoadItemSprite(string itemId)
        {
            var config = _moduleItemsManager.GetConfig(itemId);
            if (!config.HasValue)
            {
                return Optional<Sprite>.Fail();
            }
        
            return LoadItemSprite(config.Value);
        }

        public Optional<Sprite> LoadItemSprite(IModuleItem item)
        {
            var tileSprite = item.GetConfigModule<SpriteModuleConfig>();
            if (!tileSprite.HasValue)
            {
                HLogger.LogError($"SpriteModuleConfig not found in item {item.Id}");
                return Optional<Sprite>.Fail();
            }
            
            return LoadItemSprite(tileSprite.Value);
        }

        public Optional<Sprite> LoadItemSprite(IModuleItemConfig config)
        {
            var tileSprite = config.GetModule<SpriteModuleConfig>();
            if (!tileSprite.HasValue)
            {
                return Optional<Sprite>.Fail();
            }
            
            return LoadItemSprite(tileSprite.Value);
        }
        
        public Optional<Sprite> LoadItemSprite(SpriteModuleConfig config)
        {
            return _spriteLoader.Load(config.Key);
        }

        public Optional<Sprite> Load(string key)
        {
            return _spriteLoader.Load(key);
        }
    }
}