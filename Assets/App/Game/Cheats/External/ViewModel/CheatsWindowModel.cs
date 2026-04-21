using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.Services;
using App.Game.Cheats.External.View;
using App.Game.Inventory.Runtime;
using UnityEngine;

namespace App.Game.Cheats.External.ViewModel
{
    public class CheatsWindowModel
    {
        private const string m_GroundOption = "Ground";
        private const string m_InventoryOption = "Inventory";
        private readonly List<string> m_PlaceItemsDropdownOptions = new List<string>
        {
            m_InventoryOption,
            m_GroundOption,
        };
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvasController _canvasController;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly IInventoryController m_InventoryController;
        private readonly IReadOnlyList<IModuleItemConfig> m_Configs;

        private CheatsWindow m_Window;
        
        private ListPool<CheatsSlotViewModel> m_Slots;
        private List<CheatsSlotViewModel> m_ActiveSlots;

        public CheatsWindowModel(
            IAssetManager assetManager, 
            ICanvasController canvasController, 
            ISpriteLoader spriteLoader, 
            IInventoryController inventoryController,
            IReadOnlyList<IModuleItemConfig> configs)
        {
            m_AssetManager = assetManager;
            _canvasController = canvasController;
            m_SpriteLoader = spriteLoader;
            m_Configs = configs;
            m_InventoryController = inventoryController;
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
            var windowCreator = new CheatsWindowCreator(m_AssetManager, _canvasController);
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
            var dropdown = m_Window.CreateItemDropdown;
            dropdown.ClearOptions();
            dropdown.AddOptions(m_PlaceItemsDropdownOptions);
            
            m_Slots = new ListPool<CheatsSlotViewModel>(CreateSlot, 32);
            m_ActiveSlots = new List<CheatsSlotViewModel>();

            ShowItems();
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

        private void ShowItems()
        {
            foreach (var itemConfig in m_Configs)
            {
                var slot = m_Slots.Get();
                slot.Value.SetItem(itemConfig);
                m_ActiveSlots.Add(slot.Value);
            }
        }
    }
}