using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Game.Canvases.External;
using App.Game.ContainerWindow.External.ViewModel.Fabric;
using App.Game.ContainerWindow.External.ViewModel.Slots;

namespace App.Game.ContainerWindow.External.ViewModel
{
    public class ContainerWindowModel : IDisposable
    {
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private View.ContainerWindow m_Window;
        private ContainerSlotsModel m_SlotsModel;

        public ContainerWindowModel(
            IAssetManager assetManager,
            ICanvas canvas,
            ISpriteLoader spriteLoader)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
            m_SpriteLoader = spriteLoader;
        }

        public void Open(Container.Runtime.Container container)
        {
            if (m_Window == null)
            {
                if (!CreateWindow())
                {
                    HLogger.LogError("Failed to create Container window.");
                    return;
                }
            }

            m_Window.SetActive(true);
            ShowContainer(container);
        }

        private void ShowContainer(Container.Runtime.Container container)
        {
            m_SlotsModel.ShowContainer(container);
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
            var windowCreator = new ContainerWindowCreator(m_AssetManager, m_Canvas);
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
            InitSlots();
            
            m_Window.SetCloseButtonClickCallback(OnCloseButtonClick);
        }
        
        private void OnCloseButtonClick()
        {
            Close();
        }

        private void InitSlots()
        {
            m_SlotsModel = new ContainerSlotsModel(
                new ContainerSlotViewCreator(m_Window),
                m_SpriteLoader);
            m_SlotsModel.Initialize();
        }

        public void Dispose()
        {
            m_SlotsModel?.Dispose();
        }
    }
}