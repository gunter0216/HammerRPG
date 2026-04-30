using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Chests.Runtime.Config;
using App.Game.Modules.Chests.Runtime.Data;

namespace App.Game.Modules.Chests.Runtime
{
    public class ChestModuleSystem : IModuleSystem
    {
        private readonly Dictionary<DataReference, ChestModule> _modules;

        public ChestModuleSystem()
        {
            _modules = new Dictionary<DataReference, ChestModule>();
        }

        public bool TryGetModule(IModuleItem moduleItem, out ChestModule chestModule)
        {
            if (_modules.TryGetValue(moduleItem.ReferenceSelf, out chestModule))
            {
                return true;
            }

            return false;
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<ChestModuleConfig>(out var config))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            if (!moduleItem.TryGetDataModule<ChestModuleData>(out var data))
            {
                data = new ChestModuleData()
                {
                    State = ChestState.Open
                };
                
                moduleItem.AddDataModule(data);
            }
            
            _modules.Add(moduleItem.ReferenceSelf, new ChestModule(moduleItem, data, config));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
            _modules.Remove(moduleItem.ReferenceSelf);
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Chest;
        }
    }
}