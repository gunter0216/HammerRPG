using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.AI.Runtime.Config;

namespace App.Game.Modules.AI.Runtime
{
    public sealed class AIModuleSystem : IModuleSystem
    {
        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<AIModuleConfig>(out var config))
                return Optional<IModuleItem>.Success(moduleItem);

            if (!config.IsValid || !moduleItem.AddModule(new AIModule(config)))
                return Optional<IModuleItem>.Fail();

            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
        }

        public ModuleIndex SortIndex() => ModuleIndex.Default;
    }
}
