using System;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Pool.Runtime;
using App.Game.GameItems.Runtime;
using App.Game.GameTiles.External.Config.Model;
using App.Game.Inventory.External.View;
using App.Game.SpriteLoaders.Runtime;

namespace App.Game.Cheats.External.ViewModel
{
    public class CheatsSlotViewModel : IPoolReleaseListener, IPoolGetListener
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly CheatsSlotView m_View;
        private IModuleItemConfig m_Item;
        private event Action<CheatsSlotViewModel> m_ClickCallback;

        public IModuleItemConfig Item => m_Item;

        public CheatsSlotViewModel(
            ISpriteLoader spriteLoader, 
            CheatsSlotView view, 
            Action<CheatsSlotViewModel> clickCallback)
        {
            m_View = view;
            m_SpriteLoader = spriteLoader;
            m_ClickCallback = clickCallback;
            
            m_View.SetButtonClickCallback(OnButtonClick);
        }

        private void OnButtonClick()
        {
            m_ClickCallback?.Invoke(this);
        }

        public void SetItem(IModuleItemConfig item)
        {
            m_Item = item;
            var spriteModule = Item.GetModule<SpriteModuleConfig>();
            if (!spriteModule.HasValue)
            {
                HLogger.LogError($"Not found sprite for item {item.Id}");
                return;
            }

            var sprite = m_SpriteLoader.Load(spriteModule.Value.Key);
            if (!sprite.HasValue)
            {
                HLogger.LogError($"not found sprite");
                return;
            }
            
            m_View.SetSprite(sprite.Value);
        }

        public void SetActive(bool status)
        {
            m_View.SetActive(status);
        }
        
        public void BeforeReturnInPool()
        {
            SetActive(false);
        }

        public void OnGetFromPool()
        {
            SetActive(true);
        }
    }
}