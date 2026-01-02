using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.External;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewModel.Fabric;
using App.Game.Inventory.External.ViewModel.Group;
using App.Game.Inventory.External.ViewModel.Slots;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Group;
using App.Game.Inventory.Runtime.Item;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryWindowModel : IDisposable
    {
        private readonly IWindowManager m_WindowManager;
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private readonly IInventoryDataController m_DataController;
        private readonly IInventoryConfigController m_ConfigController;
        private readonly InventoryGroupController m_GroupController;
        private readonly InventoryItemsController m_ItemsController;

        private InventoryWindow m_Window;
        
        private List<InventoryGroupViewModel> m_GroupHeaderViewModels;
        
        private InventorySlotsModel m_SlotsModel;
        private InventoryGroupsViewModel m_GroupsViewModel;

        public InventoryWindowModel(
            IWindowManager windowManager,
            IAssetManager assetManager,
            IInventoryDataController dataController,
            IInventoryConfigController configController,
            ICanvas canvas,
            ISpriteLoader spriteLoader, 
            InventoryGroupController groupController,
            InventoryItemsController itemsController)
        {
            m_WindowManager = windowManager;
            m_AssetManager = assetManager;
            m_DataController = dataController;
            m_ConfigController = configController;
            m_Canvas = canvas;
            m_SpriteLoader = spriteLoader;
            m_GroupController = groupController;
            m_ItemsController = itemsController;
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
            CoroutineProvider.DoCoroutine(OpenCoroutine());
        }

        // todo wait rebuild grid 
        private IEnumerator OpenCoroutine()
        {
            yield return new WaitForEndOfFrame();
            ShowSelectedGroup();
        }

        public void Close()
        {
            m_Window.SetActive(false);
            m_SlotsModel.OnWindowClosed();
        }
        
        public bool IsOpen()
        {
            return m_Window != null && m_Window.IsActive();
        }
        
        private bool CreateWindow()
        {
            var windowCreator = new InventoryWindowCreator(m_AssetManager, m_Canvas);
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
            InitGroups();
            InitSlots();
            
            m_Window.SetBlockButtonActive(false);
            m_Window.SetCloseButtonClickCallback(OnCloseButtonClick);
        }
        
        private void OnCloseButtonClick()
        {
            Close();
        }

        private void InitGroups()
        {
            m_GroupsViewModel = new InventoryGroupsViewModel(
                m_ConfigController, 
                new InventoryGroupViewCreator(m_Window), 
                m_SpriteLoader, 
                OnGroupClick);
            m_GroupsViewModel.Initialize();
        }

        private void InitSlots()
        {
            m_SlotsModel = new InventorySlotsModel(
                m_ConfigController, 
                new InventorySlotViewCreator(m_Window),
                m_ItemsController,
                m_SpriteLoader,
                m_Window);
            m_SlotsModel.Initialize();
            m_Window.ItemsContent.transform.SetAsLastSibling();
        }

        private void OnGroupClick(InventoryGroupViewModel _)
        {
            ShowSelectedGroup();
        }

        private void ShowSelectedGroup()
        {
            var selectedGroup = m_GroupsViewModel.GetSelectedGroup();
            m_SlotsModel.ShowGroup(selectedGroup.Group);
        }

        public void AddItem(InventoryItem item)
        {
            if (!IsOpen())
            {
                return;
            }

            var group = m_GroupController.GetItemGroup(item.Item);
            if (!group.HasValue)
            {
                HLogger.LogError("group not found.");
                return;
            }

            var selectedGroup = m_GroupsViewModel.GetSelectedGroup();
            if (selectedGroup.Group != group.Value)
            {
                return;
            }
            
            m_SlotsModel.UpdateSlot(item);
        }

        public void Dispose()
        {
            m_SlotsModel?.Dispose();
        }
    }
}