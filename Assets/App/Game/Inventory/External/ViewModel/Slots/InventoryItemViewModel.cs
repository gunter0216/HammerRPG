using App.Common.Logger.Runtime;
using App.Game.GameTiles.External.Config.Model;
using App.Game.Inventory.External.View;
using App.Game.SpriteLoaders.Runtime;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryItemViewModel
    {
        private readonly InventoryItemView m_View;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private InventoryItem m_Item;

        public InventoryItemViewModel(InventoryItemView view, ISpriteLoader spriteLoader)
        {
            m_View = view;
            m_SpriteLoader = spriteLoader;
        }
        
        public void SetActive(bool isActive)
        {
            m_View.SetActive(isActive);
        }

        public void SetItem(InventoryItem item)
        {
            m_Item = item;
            var spriteModule = m_Item.Item.GetConfigModule<SpriteModuleConfig>();
            if (!spriteModule.HasValue)
            {
                HLogger.LogError("SpriteModuleConfig is not available for the item.");
                return;
            }
            
            var sprite = m_SpriteLoader.Load(spriteModule.Value.Key);
            if (!sprite.HasValue)
            {
                HLogger.LogError($"Failed to load sprite for item");
                return;
            }
            
            m_View.SetIcon(sprite.Value);
        }

        public void SetPosition(Vector2 position)
        {
            m_View.SetLocalPosition(position);
        }
    }
}