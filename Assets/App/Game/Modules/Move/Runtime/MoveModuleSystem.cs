using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Move.Runtime.Config;
using App.Game.Modules.Move.Runtime.Data;

namespace App.Game.Modules.Move.Runtime
{
    public class MoveModuleSystem : IModuleSystem
    {
        private readonly Dictionary<DataReference, MoveModule> _modules;

        public MoveModuleSystem()
        {
            _modules = new Dictionary<DataReference, MoveModule>();
        }

        public bool TryGetModule(IModuleItem moduleItem, out MoveModule moveModule)
        {
            if (_modules.TryGetValue(moduleItem.ReferenceSelf, out moveModule))
            {
                return true;
            }

            return false;
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<MoveModuleConfig>(out var config))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            if (!moduleItem.TryGetDataModule<MoveModuleData>(out var data))
            {
                data = new MoveModuleData()
                {
                };
                
                moduleItem.AddDataModule(data);
            }
            
            _modules.Add(moduleItem.ReferenceSelf, new MoveModule(moduleItem, data, config));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
            _modules.Remove(moduleItem.ReferenceSelf);
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Move;
        }
    }
}