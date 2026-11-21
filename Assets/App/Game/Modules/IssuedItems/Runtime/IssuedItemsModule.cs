using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using ItemContainerModule = App.Game.Modules.ContainerModule.Runtime.ContainerModule;
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

        public bool TryIssueItems(IModuleItemsManager itemsManager, ItemContainerModule container)
        {
            if (itemsManager == null || container == null || _config.Items == null || _data.IssuedCount < 0)
                return false;

            for (int i = _data.IssuedCount; i < _config.Items.Count; i++)
            {
                if (_config.Items[i] == null || string.IsNullOrWhiteSpace(_config.Items[i].Id))
                    return false;
            }

            while (_data.IssuedCount < _config.Items.Count)
            {
                var itemConfig = _config.Items[_data.IssuedCount];
                var item = itemsManager.Create(itemConfig.Id);
                if (!item.HasValue)
                    return false;

                if (!container.AddItem(item.Value))
                {
                    if (!itemsManager.Destroy(item.Value))
                        HLogger.LogError($"Could not clean up unissued item '{itemConfig.Id}'.");
                    return false;
                }

                _data.IssuedCount++;
            }

            return true;
        }
    }
}
