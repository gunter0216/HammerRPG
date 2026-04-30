using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;

namespace App.Common.ModuleItem.Runtime.Services
{
    public class ModuleItemDestroyer
    {
        private readonly IReadOnlyList<IDestroyModuleItemHandler> _handlers;

        public ModuleItemDestroyer(
            IReadOnlyList<IDestroyModuleItemHandler> handlers)
        {
            _handlers = handlers;
        }

        public bool Destroy(IModuleItem item)
        {
            if (item == null)
            {
                HLogger.LogError("Item is null");
                return false;
            }

            if (item is not ModuleItem moduleItem)
            {
                HLogger.LogError($"Cant convert");
                return false;
            }

            foreach (var handler in _handlers)
            {
                handler.OnItemDestroyed(item);
            }

            return moduleItem.Destroy();
        }
    }
}