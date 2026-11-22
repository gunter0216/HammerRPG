using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.ContainerItem.Runtime.Config;
using App.Game.Modules.ContainerItem.Runtime.Data;

namespace App.Game.Modules.ContainerItem.Runtime
{
    public class ContainerItemModuleSystem : IModuleSystem
    {
        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<ContainerItemModuleConfig>(out var config))
                return Optional<IModuleItem>.Success(moduleItem);

            if (!config.IsValid)
                return Optional<IModuleItem>.Fail();

            if (!moduleItem.TryGetDataModule<ContainerItemModuleData>(out var data))
            {
                data = new ContainerItemModuleData();
                if (!moduleItem.AddDataModule(data))
                    return Optional<IModuleItem>.Fail();
            }

            if (!moduleItem.AddModule(new ContainerItemModule(config, data)))
                return Optional<IModuleItem>.Fail();

            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem) { }

        public ModuleIndex SortIndex() => ModuleIndex.Default;
    }
}
