using App.Common.Logger.Runtime;
using App.Game.Equipment.External.View;
using App.Game.Equipment.External.ViewModel.Fabric;

namespace App.Game.Equipment.External.ViewModel
{
    public class EquipmentWindowModel
    {
        private EquipmentWindow m_Window;
        private readonly EquipmentWindowCreator m_WindowCreator;

        public EquipmentWindowModel(EquipmentWindowCreator windowCreator)
        {
            m_WindowCreator = windowCreator;
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

        private void InitWindow()
        {
            // Инициализация окна, если потребуется
        }
    }
}
