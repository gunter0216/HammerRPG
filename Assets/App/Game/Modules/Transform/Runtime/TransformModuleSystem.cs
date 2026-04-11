using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Transform.Runtime.Config;
using App.Game.Modules.Transform.Runtime.Data;

namespace App.Game.Modules.Transform.Runtime
{
    public class TransformModuleSystem : IModuleSystem
    {
        public TransformModuleSystem()
        {
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<TransformModuleConfig>(out var config))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            if (!moduleItem.HasDataModule<TransformModuleData>())
            {
                moduleItem.AddDataModule(new TransformModuleData());
            }
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Transform;
        }
    }
}