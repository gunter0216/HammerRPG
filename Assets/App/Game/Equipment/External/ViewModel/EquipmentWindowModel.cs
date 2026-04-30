using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Game.Equipment.External.View;
using App.Game.Equipment.External.ViewModel.Fabric;
using App.Game.Equipment.Runtime.Items;

namespace App.Game.Equipment.External.ViewModel
{
    public class EquipmentWindowModel
    {
        private readonly EquipmentWindowCreator m_WindowCreator;
        private readonly EquipmentSlotsController m_SlotsController;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private EquipmentWindow m_Window;
        private EquipmentSlotsModel m_SlotsModel;

        public EquipmentWindowModel(
            EquipmentWindowCreator windowCreator, 
            EquipmentSlotsController slotsController, 
            ISpriteLoader spriteLoader)
        {
            m_WindowCreator = windowCreator;
            m_SlotsController = slotsController;
            m_SpriteLoader = spriteLoader;
        }

        private void InitWindow()
        {
            m_SlotsModel = new EquipmentSlotsModel(
                m_Window.SlotsView, 
                m_SpriteLoader,
                m_SlotsController);
            m_SlotsModel.Initialize();
        }

        public void Open()
        {
            if (m_Window == null)
            {
                if (!CreateWindow())
                {
                    HLogger.LogError("Failed to create equipment window.");
                    return;
                }

                InitWindow();
            }

            m_Window.SetActive(true);
        }

        public void Close()
        {
            if (m_Window != null)
            {
                m_Window.SetActive(false);
            }
        }

        public bool IsOpen()
        {
            return m_Window != null && m_Window.IsActive();
        }

        private bool CreateWindow()
        {
            var windowOptional = m_WindowCreator.Create();
            if (!windowOptional.HasValue)
            {
                return false;
            }

            m_Window = windowOptional.Value;
            return true;
        }
    }
}
