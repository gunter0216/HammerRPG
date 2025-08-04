using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.Canvases.External;
using App.Game.Cheats.External.Services;
using App.Game.Cheats.External.View;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Config;
using App.Game.SpriteLoaders.Runtime;
using App.Generation.DungeonGenerator.Runtime.Matrix;
using UnityEngine;

namespace App.Game.Cheats.External.ViewModel
{
    public class CheatsWindowModel
    {
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly IReadOnlyList<IModuleItemConfig> m_Configs;
        private readonly IReadOnlyList<IInventoryGroupConfig> m_Groups;

        private CheatsWindow m_Window;
        
        private List<CheatsGroupHeaderViewModel> m_GroupHeaderViewModels;
        private Matrix<CheatsSlotViewModel> m_Slots;
        
        private CheatsGroupHeaderViewModel m_SelectedGroup;

        public CheatsWindowModel(
            IAssetManager assetManager, 
            ICanvas canvas, 
            ISpriteLoader spriteLoader, 
            IReadOnlyList<IModuleItemConfig> configs, 
            IReadOnlyList<IInventoryGroupConfig> groups)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
            m_SpriteLoader = spriteLoader;
            m_Configs = configs;
            m_Groups = groups;
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
            CreateSlots();
            
            SelectGroup(m_GroupHeaderViewModels[0]);
        }

        private void CreateSlots()
        {
            // var rows = m_ConfigController.GetRows();
            // var columns = m_ConfigController.GetCols();
            // m_Slots = new Matrix<CheatsSlotViewModel>(columns, rows);
            // for (int row = 0; row < rows; ++row)
            // {
            //     for (int col = 0; col < columns; ++col)
            //     {
            //         var view = Object.Instantiate(
            //             m_Window.CheatsSlotViewPrefab,
            //             m_Window.SlotsContent);
            //         
            //         var viewModel = new CheatsSlotViewModel(view);
            //         m_Slots.SetCell(row, col, viewModel);
            //     }
            // }
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
        }
    }
}