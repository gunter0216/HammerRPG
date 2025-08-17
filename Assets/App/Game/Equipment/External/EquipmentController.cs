using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Configs.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Canvases.External;
using App.Game.Contexts;
using App.Game.Equipment.External.ViewModel;
using App.Game.Equipment.External.ViewModel.Fabric;
using App.Game.Equipment.Runtime;
using App.Game.Equipment.Runtime.Config;
using App.Game.States.Runtime.Game;

namespace App.Game.Equipment.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 1000)]
    public class EquipmentController : IInitSystem, IEquipmentController
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly PopupCanvas m_PopupCanvas;
        [Inject] private readonly IAssetManager m_AssetManager;
        
        private EquipmentConfigController m_ConfigController;
        private EquipmentWindowModel m_EquipmentWindowModel;

        public void Init()
        {
            InitConfig();
            InitWindow();
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
