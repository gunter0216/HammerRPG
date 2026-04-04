using System;
using System.Collections.Generic;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.ContainerWindow.External.ViewModel.Fabric;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using App.Game.GameTiles.External.Config.Model;
using UnityEngine;

namespace App.Game.ContainerWindow.External.ViewModel.Slots
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
            m_Container = container;
            HideAllSlots();
            
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

                var item = container.Items[i];
                if (item != null)
                {
                    var sprite = GetSprite(item);
                    if (!sprite.HasValue)
                    {
                        HLogger.LogError("Cant load sprite");
                        return;
                    }
                    
                    slot.Value.SetItem(item, sprite.Value);
                }
                
                slot.Value.SetIndex(i);
                
                m_Slots.Add(slot.Value);
            }
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
                removeFunc: Remove);
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

            UpdateSlot(slot, item);
            m_Container.AddItem(item, slot.Index);
            
            return true;
        }

        private bool CanPlace(ItemSlotController controller, IModuleItem item)
        {
            return true;
        }
        
        private void OnSlotClick(ItemSlotController controller)
        {
            EventManager.Trigger(new ItemSlotClickEvent(controller));
        }

        private void UpdateSlot(ItemSlotController slot, IModuleItem item)
        {
            var sprite = GetSprite(item);
            slot.SetItem(item, sprite.Value);
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
            
            var sprite = m_SpriteLoader.Load(spriteModule.Value.Key);
            if (!sprite.HasValue)
            {
                HLogger.LogError($"Failed to load sprite for item");
                return Optional<Sprite>.Fail();
            }

            return sprite;
        }

        public void Dispose()
        {
        }
    }
}