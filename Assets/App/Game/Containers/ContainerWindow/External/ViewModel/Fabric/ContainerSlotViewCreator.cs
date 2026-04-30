using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.View;
using UnityEngine;

namespace App.Game.Containers.ContainerWindow.External.ViewModel.Fabric
{
    public class ContainerSlotViewCreator
    {
        private readonly View.ContainerWindow m_Window;

        public ContainerSlotViewCreator(View.ContainerWindow window)
        {
            m_Window = window;
        }

        public Optional<ItemSlotView> Create()
        {
            var view = Object.Instantiate(
                m_Window.ItemSlotViewPrefab,
                m_Window.SlotsContent);
            if (view == null)
            {
                return Optional<ItemSlotView>.Fail();
            }
            
            return Optional<ItemSlotView>.Success(view);
        }
    }
}