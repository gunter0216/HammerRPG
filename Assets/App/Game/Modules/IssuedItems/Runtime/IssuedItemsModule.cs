using System;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Container.Runtime;
using App.Game.Modules.IssuedItems.Runtime.Config;
using App.Game.Modules.IssuedItems.Runtime.Data;

namespace App.Game.Modules.IssuedItems.Runtime
{
    public class IssuedItemsModule : IModule
    {
        private readonly IssuedItemsModuleConfig _config;
        private readonly IssuedItemsModuleData _data;

        public int IssuedCount => _data.IssuedCount;

        public IssuedItemsModule(IssuedItemsModuleConfig config, IssuedItemsModuleData data)
        {
            _config = config;
            _data = data;
        }

        public bool TryIssueItems(IModuleItemsManager itemsManager, ContainerModule container,
            Action<IModuleItem> onItemIssued = null)
        {
            if (itemsManager == null || container == null || _config.Items == null || _data.IssuedCount < 0)
                return false;

            long totalQuantity = 0;
            foreach (var entry in _config.Items)
            {
                if (entry == null || entry.Item == null || entry.Quantity < 1 || string.IsNullOrWhiteSpace(entry.Item.Id))
                    return false;

                totalQuantity += entry.Quantity;
                if (totalQuantity > int.MaxValue)
                    return false;
            }

            int alreadyIssued = _data.IssuedCount;
            foreach (var entry in _config.Items)
            {
                int issuedInEntry = Math.Min(alreadyIssued, entry.Quantity);
                alreadyIssued -= issuedInEntry;
                string itemId = entry.Item.Id;

                for (int i = issuedInEntry; i < entry.Quantity; i++)
                {
                    var item = itemsManager.Create(itemId);
                    if (!item.HasValue)
                        return false;

                    if (!container.AddItem(item.Value))
                    {
                        if (!itemsManager.Destroy(item.Value))
                            HLogger.LogError($"Could not clean up unissued item '{itemId}'.");
                        return false;
                    }

                    _data.IssuedCount++;
                    onItemIssued?.Invoke(item.Value);
                }
            }

            return true;
        }
    }
}
