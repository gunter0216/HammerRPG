using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Game.GameTiles.External.Config.Model;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class ItemViewModel
    {
        private readonly ItemView m_View;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private IModuleItem m_Item;

        public ItemViewModel(
            ItemView view, 
            ISpriteLoader spriteLoader)
        {
            m_View = view;
            m_SpriteLoader = spriteLoader;
        }

        public void SetActive(bool isActive)
        {
            m_View.SetActive(isActive);
            if (isActive)
            {
                m_View.SetAsLastSibling();
            }
        }

        public void SetItem(IModuleItem item)
        {
            var spriteModule = item.GetConfigModule<SpriteModuleConfig>();
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

        public void SetAsLastSibling()
        {
            m_View.SetAsLastSibling();
        }
        
        public void SetScale(float scale)
        {
            m_View.SetScale(scale);
        }

        public void SetLocalPosition(Vector2 position)
        {
            m_View.SetLocalPosition(position);
        }
        
        public void SetPosition(Vector2 position)
        {
            m_View.SetPosition(position);
        }

        public void SetParent(Transform parent)
        {
            m_View.SetParent(parent);
        }
    }
}