using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.Services;
using App.Game.Cheats.External.View;
using App.Game.Inventory.External;
using App.Game.Inventory.Runtime.Config;
using App.Game.SpriteLoaders.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;
using Assets.App.Game.GameItems.Runtime;
using UnityEngine;

namespace App.Game.Cheats.External.ViewModel
{
    public class CheatsWindowModel
    {
        private const string m_GroundOption = "Ground";
        private const string m_InventoryOption = "Inventory";
        private readonly List<string> m_PlaceItemsDropdownOptions = new List<string>
        {
            m_GroundOption,
            m_InventoryOption,
        };
        
        private readonly IGameItemsManager m_GameItemsManager;
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly IInventoryController m_InventoryController;
        private readonly IReadOnlyList<IModuleItemConfig> m_Configs;
        private readonly IReadOnlyList<IInventoryGroupConfig> m_Groups;

        private CheatsWindow m_Window;
        
        private List<CheatsGroupHeaderViewModel> m_GroupHeaderViewModels;
        private ListPool<CheatsSlotViewModel> m_Slots;
        private List<CheatsSlotViewModel> m_ActiveSlots;
        
        private CheatsGroupHeaderViewModel m_SelectedGroup;

        public CheatsWindowModel(
            IAssetManager assetManager, 
            ICanvas canvas, 
            ISpriteLoader spriteLoader, 
            IGameItemsManager gameItemsManager,
            IInventoryController inventoryController,
            IReadOnlyList<IModuleItemConfig> configs, 
            IReadOnlyList<IInventoryGroupConfig> groups)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
            m_SpriteLoader = spriteLoader;
            m_Configs = configs;
            m_Groups = groups;
            m_InventoryController = inventoryController;
            m_GameItemsManager = gameItemsManager;
        }

        public void Open()
        {
            if (m_Window == null)
            {
                if (!CreateWindow())
                {
                    HLogger.LogError("Failed to create inventory window.");
                    return;
                }
            }
            
            m_Window.SetActive(true);
        }
        
        public void Close()
        {
            m_Window.SetActive(false);
        }
        
        public bool IsOpen()
        {
            return m_Window != null && m_Window.IsActive();
        }
        
        private bool CreateWindow()
        {
            var windowCreator = new CheatsWindowCreator(m_AssetManager, m_Canvas);
            var window = windowCreator.Create();
            if (!window.HasValue)
            {
                return false;
            }

            m_Window = window.Value;
            InitWindow();
            
            return true;
        }

        private void InitWindow()
        {
            CreateGroups();

            var dropdown = m_Window.CreateItemDropdown;
            dropdown.ClearOptions();
            dropdown.AddOptions(m_PlaceItemsDropdownOptions);
            
            m_Slots = new ListPool<CheatsSlotViewModel>(CreateSlot, 32);
            m_ActiveSlots = new List<CheatsSlotViewModel>();
            
            SelectGroup(m_GroupHeaderViewModels[0]);
        }

        private Optional<CheatsSlotViewModel> CreateSlot()
        {
            var view = Object.Instantiate(
                m_Window.CheatsSlotViewPrefab,
                m_Window.SlotsContent);
                    
            var viewModel = new CheatsSlotViewModel(m_SpriteLoader, view, OnSlotClick);
            return Optional<CheatsSlotViewModel>.Success(viewModel);
        }

        private void OnSlotClick(CheatsSlotViewModel viewModel)
        {
            var value = m_PlaceItemsDropdownOptions[m_Window.CreateItemDropdown.value];
            if (value == m_InventoryOption)
            {
                m_InventoryController.AddItem(viewModel.Item);
            }
            else if (value == m_GroundOption)
            {
                HLogger.LogError("not implemented yet: place item on ground");
            }
        }

        private void CreateGroups()
        {
            var groups = m_Groups;
            m_GroupHeaderViewModels = new List<CheatsGroupHeaderViewModel>(groups.Count);
            foreach (var group in groups)
            {
                var view = Object.Instantiate(
                    m_Window.CheatsGroupHeaderViewPrefab,
                    m_Window.HeaderGroupContent);
                
                var viewModel = new CheatsGroupHeaderViewModel(
                    view, 
                    group, 
                    m_SpriteLoader,
                    OnGroupClick);
                viewModel.Initialize();
                m_GroupHeaderViewModels.Add(viewModel);
            }
        }

        private void OnGroupClick(CheatsGroupHeaderViewModel viewModel)
        {
            if (m_SelectedGroup == viewModel)
            {
                return;
            }
            
            SelectGroup(viewModel);
        }

        private void SelectGroup(CheatsGroupHeaderViewModel viewModel)
        {
            m_SelectedGroup = viewModel;
            foreach (var groupViewModel in m_GroupHeaderViewModels)
            {
                groupViewModel.SetActiveStatus(groupViewModel == viewModel);
            }

            for (int i = 0; i < m_ActiveSlots.Count; ++i)
            {
                m_Slots.Release(m_ActiveSlots[i]);
            }
            
            m_ActiveSlots.Clear();
            
            var items = m_GameItemsManager.GetItemsByType(m_SelectedGroup.Group.GameType);
            if (!items.HasValue)
            {
                return;
            }

            foreach (var itemConfig in items.Value)
            {
                var slot = m_Slots.Get();
                slot.Value.SetItem(itemConfig);
                m_ActiveSlots.Add(slot.Value);
            }
        }
    }
}