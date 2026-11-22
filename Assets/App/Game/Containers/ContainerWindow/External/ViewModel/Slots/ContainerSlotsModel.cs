using System;
using System.Collections.Generic;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Containers.ContainerWindow.External.ViewModel.Fabric;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using App.Game.Modules.Sprite.Runtime;
using UnityEngine;

namespace App.Game.Containers.ContainerWindow.External.ViewModel.Slots
{
    public class ContainerSlotsModel : IDisposable
    {
        private readonly ContainerSlotViewCreator m_SlotViewCreator;
        private readonly ISpriteLoader m_SpriteLoader;

        private ListPool<ItemSlotController> m_SlotsPool;
        private List<ItemSlotController> m_Slots;
        private Container.Runtime.Container m_Container;

        public ContainerSlotsModel(
            ContainerSlotViewCreator slotViewCreator, 
            ISpriteLoader spriteLoader)
        {
            m_SlotViewCreator = slotViewCreator;
            m_SpriteLoader = spriteLoader;
        }

        public void Initialize()
        {
            m_Slots = new List<ItemSlotController>(27);
            m_SlotsPool = new ListPool<ItemSlotController>(
                createFunc: CreateSlot,
                capacity: 27,
                getCallback: x => x.SetActive(true),
                releaseCallback: x => x.SetActive(false));
        }
        
        public void ShowContainer(Container.Runtime.Container container)
        {
            HideContainer();
            m_Container = container;
            
            for (int i = 0; i < container.Items.Count; ++i)
            {
                var slot = m_SlotsPool.Get();
                if (!slot.HasValue)
                {
                    HLogger.LogError("Cant create slot");
                    return;
                }
                
                slot.Value.Clear();
                slot.Value.SetAsLastSibling();

                slot.Value.SetIndex(i);
                m_Slots.Add(slot.Value);
                UpdateSlot(i);
            }
            m_Container.ItemChanged += UpdateSlot;
        }

        public void HideContainer()
        {
            if (m_Container == null)
                return;
            m_Container.ItemChanged -= UpdateSlot;
            m_Container = null;
            HideAllSlots();
        }

        private Optional<ItemSlotController> CreateSlot()
        {
            var view = m_SlotViewCreator.Create();
            if (!view.HasValue)
            {
                HLogger.LogError($"Failed to create slot view");
                return Optional<ItemSlotController>.Fail();
            }
                    
            var viewModel = new ItemSlotController(
                view.Value,
                0,
                clickCallback: OnSlotClick,
                canPlaceFunc: CanPlace,
                placeFunc: Place,
                removeFunc: Remove,
                getContainer: () => m_Container,
                getSlot: index => m_Slots[index]);
            viewModel.Initialize();
            
            return Optional<ItemSlotController>.Success(viewModel);
        }

        private Optional<IModuleItem> Remove(ItemSlotController slot)
        {
            var item = slot.GetItem();
            if (item == null)
            {
                HLogger.LogError("Slot is empty");
                return Optional<IModuleItem>.Fail();
            }

            slot.Clear();
            m_Container.Remove(slot.Index);
            
            return Optional<IModuleItem>.Success(item);
        }

        private bool Place(ItemSlotController slot, IModuleItem item)
        {
            if (slot.HasItem())
            {
                HLogger.LogError($"slot is not empty {slot.GetItem().Id}");
                return false;
            }

            return m_Container.AddItem(item, slot.Index);
        }

        private bool CanPlace(ItemSlotController controller, IModuleItem item)
        {
            return m_Container?.CanPlace(item, controller.Index, controller.GetItem()) == true;
        }
        
        private void OnSlotClick(ItemSlotController controller)
        {
            EventManager.Trigger(new ItemSlotClickEvent(controller));
        }

        private void UpdateSlot(int index)
        {
            var slot = m_Slots[index];
            var item = m_Container.Items[index];
            if (item == null)
            {
                slot.Clear();
                return;
            }
            var sprite = GetSprite(item);
            slot.SetItem(item, sprite.HasValue ? sprite.Value : null);
        }

        private void HideAllSlots()
        {
            foreach (var slot in m_Slots)
            {
                m_SlotsPool.Release(slot);
            }
            
            m_Slots.Clear();
        }

        private Optional<Sprite> GetSprite(IModuleItem item)
        {
            var spriteModule = item.GetConfigModule<SpriteModuleConfig>();
            if (!spriteModule.HasValue)
            {
                HLogger.LogError("SpriteModuleConfig is not available for the item.");
                return Optional<Sprite>.Fail();
            }

            return Optional<Sprite>.Success(spriteModule.Value.Sprite);
        }

        public void Dispose()
        {
            HideContainer();
        }
    }
}
