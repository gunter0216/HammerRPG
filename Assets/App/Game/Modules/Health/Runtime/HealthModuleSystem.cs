using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Health.Runtime.Config;
using App.Game.Modules.Health.Runtime.Data;

namespace App.Game.Modules.Health.Runtime
{
    public class HealthModuleSystem : IModuleSystem
    {
        public HealthModuleSystem()
        {
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<HealthModuleConfig>(out var config))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            if (!moduleItem.TryGetDataModule<HealthModuleData>(out var data))
            {
                data = new HealthModuleData
                {
                    Health = config.MaxHealth
                };
                
                moduleItem.AddDataModule(data);
            }

            moduleItem.AddModule(new HealthModule(moduleItem, data, config));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Health;
        }
    }
}