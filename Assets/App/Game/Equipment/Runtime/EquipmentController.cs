using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime;
using App.Common.Data.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Equipment.External.ViewModel;
using App.Game.Equipment.External.ViewModel.Fabric;
using App.Game.Equipment.Runtime;
using App.Game.Equipment.Runtime.Config;
using App.Game.Equipment.Runtime.Data;

namespace App.Game.Equipment.External
{
    public class EquipmentController : IInitSystem, IEquipmentController
    {
        private readonly IConfigLoader m_ConfigLoader;
        private readonly IDataManager m_DataManager;
        private readonly PopupCanvas m_PopupCanvas;
        private readonly IAssetManager m_AssetManager;
        
        private EquipmentConfigController m_ConfigController;
        private EquipmentDataController m_DataController;
        private EquipmentWindowModel m_EquipmentWindowModel;

        public EquipmentController(
            IConfigLoader configLoader, 
            PopupCanvas popupCanvas, 
            IAssetManager assetManager, 
            IDataManager dataManager)
        {
            m_ConfigLoader = configLoader;
            m_PopupCanvas = popupCanvas;
            m_AssetManager = assetManager;
            m_DataManager = dataManager;
        }

        public void Init()
        {
            InitData();
            InitConfig();
            InitWindow();
        }

        private void InitData()
        {
            m_DataController = new EquipmentDataController(m_DataManager);
            m_DataController.Initialize();
        }

        private bool InitConfig()
        {
            m_ConfigController = new EquipmentConfigController(m_ConfigLoader);
            return m_ConfigController.Initialize();
        }

        private void InitWindow()
        {
            var windowCreator = new EquipmentWindowCreator(m_AssetManager, m_PopupCanvas);
            m_EquipmentWindowModel = new EquipmentWindowModel(windowCreator);
        }

        public void OpenWindow()
        {
            m_EquipmentWindowModel.Open();
        }

        public void CloseWindow()
        {
            m_EquipmentWindowModel.Close();
        }

        public bool IsOpen()
        {
            return m_EquipmentWindowModel.IsOpen();
        }
    }
}
