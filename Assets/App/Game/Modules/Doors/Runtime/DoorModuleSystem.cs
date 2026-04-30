using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Doors.Runtime.Config;
using App.Game.Modules.Doors.Runtime.Data;

namespace App.Game.Modules.Doors.Runtime
{
    public class DoorModuleSystem : IModuleSystem
    {
        private readonly Dictionary<DataReference, DoorModule> _modules;

        public DoorModuleSystem()
        {
            _modules = new Dictionary<DataReference, DoorModule>();
        }

        public bool TryGetModule(IModuleItem moduleItem, out DoorModule DoorModule)
        {
            if (_modules.TryGetValue(moduleItem.ReferenceSelf, out DoorModule))
            {
                return true;
            }

            return false;
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<DoorModuleConfig>(out var config))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            if (!moduleItem.TryGetDataModule<DoorModuleData>(out var data))
            {
                data = new DoorModuleData()
                {
                    State = DoorState.Open
                };
                
                moduleItem.AddDataModule(data);
            }
            
            _modules.Add(moduleItem.ReferenceSelf, new DoorModule(moduleItem, data, config));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
            _modules.Remove(moduleItem.ReferenceSelf);
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Door;
        }
    }
}