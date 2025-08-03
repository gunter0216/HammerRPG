using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Windows.External;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewController;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryWindowModel
    {
        private readonly IWindowManager m_WindowManager;
        private readonly IAssetManager m_AssetManager;

        private InventoryWindow m_Window;

        public InventoryWindowModel(IWindowManager windowManager, IAssetManager assetManager)
        {
            m_WindowManager = windowManager;
            m_AssetManager = assetManager;
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
        
        private bool CreateWindow()
        {
            var windowCreator = new InventoryWindowCreator(m_AssetManager);
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
            
        }
    }
}