using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Configs.Runtime;
using App.Common.Data.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Contexts;
using App.Game.Inventory.External.Config;
using App.Game.Inventory.External.Data;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Runtime.Game;

namespace App.Game.Inventory.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class InventoryController : IInitSystem, IInventoryController
    {
        [Inject] private readonly IDataManager m_DataManager;
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly IWindowManager m_WindowManager;
        [Inject] private readonly IAssetManager m_AssetManager;
        [Inject] private readonly PopupCanvas m_PopupCanvas;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;
        [Inject] private readonly IModuleItemsManager m_ModuleItemsManager;
        
        private InventoryDataController m_DataController;
        private InventoryConfigController m_ConfigController;
        private InventoryWindowModel m_InventoryWindowModel;
        
        public void Init()
        {
            InitData();
            InitConfig();
            InitWindow();
        }

        private void InitWindow()
        {
            m_InventoryWindowModel = new InventoryWindowModel(
                m_WindowManager,
                m_AssetManager,
                m_DataController,
                m_ConfigController,
                m_PopupCanvas,
                m_SpriteLoader);
        }

        private bool InitConfig()
        {
            m_ConfigController = new InventoryConfigController(m_ConfigLoader);
            return m_ConfigController.Initialize();
        }

        private bool InitData()
        {
            m_DataController = new InventoryDataController(m_DataManager);
            return m_DataController.Initialize();
        }

        public void OpenWindow()
        {
            m_InventoryWindowModel.Open();
        }

        public void CloseWindow()
        {
            m_InventoryWindowModel.Close();
        }

        public bool IsOpen()
        {
            return m_InventoryWindowModel.IsOpen();
        }

        public bool AddItem(IModuleItemConfig moduleItemConfig)
        {
            if (moduleItemConfig == null)
            {
                HLogger.LogError("Cannot add null item to inventory");
                return false;
            }

            return AddItem(moduleItemConfig.Id);
        }

        public bool AddItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                HLogger.LogError("Cannot add item with null or empty id to inventory");
                return false;
            }
            
            var item = m_ModuleItemsManager.Create(id);
            if (!item.HasValue)
            {
                HLogger.LogError($"Failed to create item with id {id}");
                return false;
            }
            
            HLogger.LogError("Adding item to inventory: " + item.Value.Id);

            // if (!m_InventoryDataController.AddItem(moduleItemConfig))
            // {
            //     HLogger.LogError($"Failed to add item {moduleItemConfig.Id} to inventory");
            //     return false;
            // }
            //
            // m_InventoryWindowModel.Refresh();
            return true;
        }

        public bool AddItem(IModuleItem moduleItem)
        {
            return true;
            // if (moduleItem == null)
            // {
            //     HLogger.LogError("Cannot add null item to inventory");
            //     return false;
            // }
            //
            // var moduleItemConfig = moduleItem.Config;
            // if (moduleItemConfig == null)
            // {
            //     HLogger.LogError($"Cannot add item {moduleItem.Id} with null config to inventory");
            //     return false;
            // }
            //
            // return AddItem(moduleItemConfig);
        }

        public IReadOnlyList<IInventoryGroupConfig> GetGroups()
        {
            return m_ConfigController.GetGroups();
        }
    }
}