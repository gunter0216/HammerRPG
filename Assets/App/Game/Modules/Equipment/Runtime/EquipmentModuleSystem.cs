using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Equipment.Runtime.Constants;
using App.Game.Modules.Equipment.Runtime.Config;
using App.Game.Modules.Equipment.Runtime.Data;

namespace App.Game.Modules.Equipment.Runtime
{
    public sealed class EquipmentModuleSystem : IModuleSystem
    {
        private readonly IModuleItemsManager _itemsManager;

        public EquipmentModuleSystem(IModuleItemsManager itemsManager)
        {
            _itemsManager = itemsManager;
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem item)
        {
            if (!item.TryGetConfigModule<EquipmentModuleConfig>(out var config))
                return Optional<IModuleItem>.Success(item);

            if (!HasValidSlots(config.Slots))
                return Optional<IModuleItem>.Fail();

            if (!item.TryGetDataModule<EquipmentModuleData>(out var data))
            {
                data = new EquipmentModuleData();
                if (!item.AddDataModule(data))
                    return Optional<IModuleItem>.Fail();
            }

            var equipment = new EquipmentModule(item, config, data, _itemsManager);
            if (!item.AddModule(equipment))
                return Optional<IModuleItem>.Fail();

            return Optional<IModuleItem>.Success(item);
        }

        private static bool HasValidSlots(IReadOnlyList<EquipmentModuleConfig.SlotRule> slots)
        {
            if (slots == null)
                return false;

            var seen = new HashSet<EquipmentSlotId>();
            foreach (var rule in slots)
            {
                if (rule == null || !Enum.IsDefined(typeof(EquipmentSlotId), rule.Slot) || !seen.Add(rule.Slot) ||
                    rule.AllowedTypes == null || rule.AllowedTypes.Count == 0)
                    return false;

                foreach (var type in rule.AllowedTypes)
                {
                    if (!Enum.IsDefined(typeof(EquipmentType), type))
                        return false;
                }
            }

            return true;
        }

        public ModuleIndex SortIndex() => ModuleIndex.Equipment;

        public void OnItemDestroyed(IModuleItem item)
        {
            if (item.TryGetModule<EquipmentModule>(out var equipment))
                equipment.DestroyEquippedItems();
        }
    }
}
