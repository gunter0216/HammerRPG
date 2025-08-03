using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewController;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.SpriteLoaders.Runtime;
using App.Generation.DungeonGenerator.Runtime.Matrix;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryWindowModel
    {
        private readonly IWindowManager m_WindowManager;
        private readonly IAssetManager m_AssetManager;
        private readonly IInventoryDataController m_DataController;
        private readonly IInventoryConfigController m_ConfigController;
        private readonly ICanvas m_Canvas;
        private readonly ISpriteLoader m_SpriteLoader;

        private InventoryWindow m_Window;
        
        private List<InventoryGroupHeaderViewModel> m_GroupHeaderViewModels;
        private Matrix<InventorySlotViewModel> m_Slots;
        
        private InventoryGroupHeaderViewModel m_SelectedGroup;

        public InventoryWindowModel(
            IWindowManager windowManager,
            IAssetManager assetManager,
            IInventoryDataController dataController,
            IInventoryConfigController configController, 
            ICanvas canvas, 
            ISpriteLoader spriteLoader)
        {
            m_WindowManager = windowManager;
            m_AssetManager = assetManager;
            m_DataController = dataController;
            m_ConfigController = configController;
            m_Canvas = canvas;
            m_SpriteLoader = spriteLoader;
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
            CreateGroups();
            CreateSlots();
            
            SelectGroup(m_GroupHeaderViewModels[0]);
        }

        private void CreateSlots()
        {
            var rows = m_ConfigController.GetRows();
            var columns = m_ConfigController.GetCols();
            m_Slots = new Matrix<InventorySlotViewModel>(columns, rows);
            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < columns; ++col)
                {
                    var view = Object.Instantiate(
                        m_Window.InventorySlotViewPrefab,
                        m_Window.SlotsContent);
                    
                    var viewModel = new InventorySlotViewModel(view);
                    m_Slots.SetCell(row, col, viewModel);
                }
            }
        }

        private void CreateGroups()
        {
            var groups = m_ConfigController.GetGroups();
            m_GroupHeaderViewModels = new List<InventoryGroupHeaderViewModel>(groups.Count);
            foreach (var group in groups)
            {
                var view = Object.Instantiate(
                    m_Window.InventoryGroupHeaderViewPrefab,
                    m_Window.HeaderGroupContent);
                
                var viewModel = new InventoryGroupHeaderViewModel(
                    view, 
                    group, 
                    m_SpriteLoader,
                    OnGroupClick);
                viewModel.Initialize();
                m_GroupHeaderViewModels.Add(viewModel);
            }
        }

        private void OnGroupClick(InventoryGroupHeaderViewModel viewModel)
        {
            if (m_SelectedGroup == viewModel)
            {
                return;
            }
            
            SelectGroup(viewModel);
        }

        private void SelectGroup(InventoryGroupHeaderViewModel viewModel)
        {
            m_SelectedGroup = viewModel;
            foreach (var groupViewModel in m_GroupHeaderViewModels)
            {
                groupViewModel.SetActiveStatus(groupViewModel == viewModel);
            }
        }
    }
}