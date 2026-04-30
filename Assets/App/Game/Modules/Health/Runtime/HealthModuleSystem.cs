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
        private readonly Dictionary<DataReference, HealthModule> _modules;

        public HealthModuleSystem()
        {
            _modules = new Dictionary<DataReference, HealthModule>();
        }

        public bool TryGetModule(IModuleItem moduleItem, out HealthModule healthModule)
        {
            if (_modules.TryGetValue(moduleItem.ReferenceSelf, out healthModule))
            {
                return true;
            }

            return false;
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
            
            _modules.Add(moduleItem.ReferenceSelf, new HealthModule(moduleItem, data, config));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
            _modules.Remove(moduleItem.ReferenceSelf);
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Health;
        }
    }
}