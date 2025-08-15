using System;
using App.Common.Logger.Runtime;
using App.Game.Inventory.External.View;
using App.Game.Inventory.Runtime.Config;
using App.Game.SpriteLoaders.Runtime;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryGroupViewModel
    {
        private readonly InventoryGroupHeaderView m_View;
        private readonly IInventoryGroupConfig m_Group;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private event Action<InventoryGroupViewModel> m_OnButtonClick;

        public IInventoryGroupConfig Group => m_Group;

        public InventoryGroupViewModel(
            InventoryGroupHeaderView view, 
            IInventoryGroupConfig group, 
            ISpriteLoader spriteLoader,
            Action<InventoryGroupViewModel> onButtonClick)
        {
            m_View = view;
            m_Group = group;
            m_SpriteLoader = spriteLoader;
            m_OnButtonClick = onButtonClick;
        }

        public void Initialize()
        {
            var icon = m_SpriteLoader.Load(Group.Icon);
            if (!icon.HasValue)
            {
                HLogger.LogError($"Failed to load icon for group: {Group.Id}");
            }
            else
            {
                m_View.SetIcon(icon.Value);
            }

            m_View.SetActiveStatus(false);
            m_View.SetButtonClickCallback(OnButtonClick);
        }

        private void OnButtonClick()
        {
            m_OnButtonClick?.Invoke(this);
        }

        public void SetActiveStatus(bool status)
        {
            m_View.SetActiveStatus(status);
        }
    }
}