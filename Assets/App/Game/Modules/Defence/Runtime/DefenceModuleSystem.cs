using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Health.Runtime;

namespace App.Game.Modules.Defence.Runtime
{
    public class DefenceModuleSystem : IModuleSystem
    {
        public DefenceModuleSystem()
        {
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetModule<HealthModule>(out var healthModule))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            moduleItem.AddModule(new DefenceModule(moduleItem, healthModule));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Defence;
        }
    }
}