using System;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Containers.Container.Runtime;
using App.Game.Equipment.Runtime.Config.Model;
using App.Game.Equipment.Runtime.Constants;
using App.Game.Modules.Container.Runtime;
using App.Game.Modules.ContainerItem.Runtime;
using App.Game.Modules.Equipment.Runtime.Config;
using App.Game.Modules.Equipment.Runtime.Data;

namespace App.Game.Modules.Equipment.Runtime
{
    public sealed class EquipmentModule : IModule
    {
        private readonly IModuleItem _owner;
        private readonly EquipmentModuleConfig _config;
        private readonly EquipmentModuleData _data;
        private readonly IModuleItemsManager _itemsManager;
        private readonly Dictionary<EquipmentSlotId, IModuleItem> _items = new();
        private bool _changingSlots;

        public EquipmentModule(IModuleItem owner, EquipmentModuleConfig config, EquipmentModuleData data,
            IModuleItemsManager itemsManager)
        {
            _owner = owner;
            _config = config;
            _data = data;
            _itemsManager = itemsManager;
        }

        public IReadOnlyList<EquipmentModuleConfig.SlotRule> Slots => _config.Slots;

        public event Action<EquipmentSlotId> SlotChanged;

        public bool CanEquip(EquipmentSlotId slot, IModuleItem item)
        {
            if (item == null || !item.TryGetConfigModule<EquipmentItemModuleConfig>(out var config) ||
                !EquipmentTypeConstants.TryGetType(config.Type, out var itemType))
                return false;

            foreach (var rule in Slots)
            {
                if (rule.Slot != slot)
                    continue;

                foreach (var type in rule.AllowedTypes)
                {
                    if (type == itemType)
                        return true;
                }

                return false;
            }

            return false;
        }

        public bool TryEquip(EquipmentSlotId slot, IModuleItem item)
        {
            if (!TryResolvePlacement(slot, item, out var source, out int index))
                return false;

            _changingSlots = true;
            try
            {
                _data.Slots.Add(slot, item.ReferenceSelf);
                _items.Add(slot, item);
                source?.Remove(index);
            }
            finally
            {
                _changingSlots = false;
            }

            SlotChanged?.Invoke(slot);
            return true;
        }

        public bool CanPlaceItem(EquipmentSlotId slot, IModuleItem item)
        {
            return TryResolvePlacement(slot, item, out _, out _);
        }

        private bool TryResolvePlacement(EquipmentSlotId slot, IModuleItem item,
            out Containers.Container.Runtime.Container source, out int index)
        {
            source = null;
            index = -1;
            if (_changingSlots || _data.Slots.ContainsKey(slot) || !CanEquip(slot, item))
                return false;

            var reference = item.ReferenceSelf;
            if (reference == null || SameItem(reference, _owner.ReferenceSelf))
                return false;

            foreach (var equipped in _data.Slots.Values)
            {
                if (SameItem(reference, equipped))
                    return false;
            }

            return TryFindSourceInventory(item, out source, out index);
        }

        public bool TryGetEquippedItem(EquipmentSlotId slot, out IModuleItem item)
        {
            if (_items.TryGetValue(slot, out item))
                return true;
            if (!_data.Slots.TryGetValue(slot, out var reference) || reference == null)
                return false;

            var restored = _itemsManager.Create(reference);
            if (!restored.HasValue)
                return false;

            item = restored.Value;
            _items.Add(slot, item);
            return true;
        }

        public bool TryUnequip(EquipmentSlotId slot, out IModuleItem item)
        {
            item = null;
            if (_changingSlots || !TryGetEquippedItem(slot, out item))
                return false;

            _items.Remove(slot);
            _data.Slots.Remove(slot);
            SlotChanged?.Invoke(slot);
            return true;
        }

        public bool CanUnequipToInventory(EquipmentSlotId slot, Containers.Container.Runtime.Container target, int index)
        {
            return TryGetItemForTransfer(slot, target, index, out _);
        }

        public bool TryUnequipToInventory(EquipmentSlotId slot, Containers.Container.Runtime.Container target, int index)
        {
            if (!TryGetItemForTransfer(slot, target, index, out var item))
                return false;

            var reference = _data.Slots[slot];
            _changingSlots = true;
            try
            {
                _items.Remove(slot);
                _data.Slots.Remove(slot);
                if (!target.AddItem(item, index))
                {
                    _data.Slots.Add(slot, reference);
                    _items.Add(slot, item);
                    return false;
                }
            }
            finally
            {
                _changingSlots = false;
            }

            SlotChanged?.Invoke(slot);
            return true;
        }

        private bool TryGetItemForTransfer(EquipmentSlotId slot, Containers.Container.Runtime.Container target,
            int index, out IModuleItem item)
        {
            item = null;
            return !_changingSlots && target != null &&
                   _owner.TryGetModule<ContainerModule>(out var inventory) && inventory.Container == target &&
                   TryGetEquippedItem(slot, out item) && target.CanPlace(item, index);
        }

        private bool TryFindSourceInventory(IModuleItem item, out Containers.Container.Runtime.Container source, out int index)
        {
            source = null;
            index = -1;
            if (!item.TryGetModule<ContainerItemModule>(out var containerItem) || containerItem.Index < 0)
                return true;
            if (!_owner.TryGetModule<ContainerModule>(out var inventory))
                return false;

            source = inventory.Container;
            index = containerItem.Index;
            return index < source.Items.Count && ReferenceEquals(source.Items[index], item);
        }

        private static bool SameItem(DataReference reference, DataReference other)
        {
            return other != null && reference.Key == other.Key && reference.Index == other.Index;
        }

        internal void DestroyEquippedItems()
        {
            _changingSlots = true;
            SlotChanged = null;
            var ownedItems = new List<IModuleItem>(_data.Slots.Count);
            foreach (var slot in _data.Slots.Keys)
            {
                if (TryGetEquippedItem(slot, out var item))
                    ownedItems.Add(item);
            }

            _items.Clear();
            _data.Slots.Clear();
            foreach (var item in ownedItems)
            {
                if (!_itemsManager.Destroy(item))
                    HLogger.LogError($"Could not destroy equipped item '{item.Id}'.");
            }
        }
    }
}
