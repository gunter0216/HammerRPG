using System;
using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Game.Inventory.External.Services;
using App.Game.Inventory.Runtime.Config;
using App.Game.SpriteLoaders.Runtime;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryGroupsViewModel
    {
        private readonly IInventoryConfigController m_ConfigController;
        private readonly InventoryGroupViewCreator m_GroupViewCreator;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly Action<InventoryGroupViewModel> m_OnButtonClick;

        private List<InventoryGroupViewModel> m_GroupHeaderViewModels;
        private InventoryGroupViewModel m_SelectedGroup;

        public InventoryGroupsViewModel(
            IInventoryConfigController configController, 
            InventoryGroupViewCreator groupViewCreator, 
            ISpriteLoader spriteLoader,
            Action<InventoryGroupViewModel> onButtonClick)
        {
            m_ConfigController = configController;
            m_GroupViewCreator = groupViewCreator;
            m_SpriteLoader = spriteLoader;
            m_OnButtonClick = onButtonClick;
        }
        
        public void Initialize()
        {
            CreateGroups();
            m_SelectedGroup = m_GroupHeaderViewModels[0];
            SelectGroup(m_SelectedGroup);
        }

        private void CreateGroups()
        {
            var groups = m_ConfigController.GetGroups();
            m_GroupHeaderViewModels = new List<InventoryGroupViewModel>(groups.Count);
            foreach (var group in groups)
            {
                var view = m_GroupViewCreator.Create();
                if (!view.HasValue)
                {
                    HLogger.LogError($"Failed to create group view");
                    return;
                }
                
                var viewModel = new InventoryGroupViewModel(
                    view.Value, 
                    group, 
                    m_SpriteLoader,
                    OnGroupClick);
                viewModel.Initialize();
                m_GroupHeaderViewModels.Add(viewModel);
            }
        }
        
        public InventoryGroupViewModel GetSelectedGroup()
        {
            return m_SelectedGroup;
        }

        private void OnGroupClick(InventoryGroupViewModel groupViewModel)
        {
            if (m_SelectedGroup == groupViewModel)
            {
                return;
            }
            
            SelectGroup(groupViewModel);
            m_OnButtonClick?.Invoke(groupViewModel);
        }

        private void SelectGroup(InventoryGroupViewModel groupViewModel)
        {
            m_SelectedGroup.SetActiveStatus(false);
            m_SelectedGroup = groupViewModel;
            m_SelectedGroup.SetActiveStatus(true);
        }
    }
}