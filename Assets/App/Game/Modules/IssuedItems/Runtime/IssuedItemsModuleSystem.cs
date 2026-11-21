using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.IssuedItems.Runtime.Config;
using App.Game.Modules.IssuedItems.Runtime.Data;

namespace App.Game.Modules.IssuedItems.Runtime
{
    public class IssuedItemsModuleSystem : IModuleSystem
    {
        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<IssuedItemsModuleConfig>(out var config))
                return Optional<IModuleItem>.Success(moduleItem);

            if (!moduleItem.TryGetDataModule<IssuedItemsModuleData>(out var data))
            {
                data = new IssuedItemsModuleData();
                if (!moduleItem.AddDataModule(data))
                    return Optional<IModuleItem>.Fail();
            }

            if (!moduleItem.AddModule(new IssuedItemsModule(config, data)))
                return Optional<IModuleItem>.Fail();

            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem) { }

        // This handler only attaches state and behaviour; issuing happens after all owner's modules are created.
        public ModuleIndex SortIndex() => ModuleIndex.Default;
    }
}
