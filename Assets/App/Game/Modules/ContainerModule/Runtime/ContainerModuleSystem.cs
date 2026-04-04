using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Containers.Container.Runtime;
using App.Game.Modules.ContainerModule.Runtime.Config;
using App.Game.Modules.ContainerModule.Runtime.Data;

namespace App.Game.Modules.ContainerModule.Runtime
{
    public class ContainerModuleSystem : IModuleSystem
    {
        private readonly IContainerController _containerController;
        private readonly Dictionary<DataReference, ContainerModule> _modules;

        public ContainerModuleSystem(IContainerController containerController)
        {
            _containerController = containerController;
            
            _modules = new Dictionary<DataReference, ContainerModule>();
        }

        public bool TryGetModule(IModuleItem moduleItem, out ContainerModule containerModule)
        {
            if (_modules.TryGetValue(moduleItem.ReferenceSelf, out containerModule))
            {
                return true;
            }

            return false;
        }

        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            if (!moduleItem.TryGetConfigModule<ContainerModuleConfig>(out var config))
            {
                return Optional<IModuleItem>.Success(moduleItem);
            }

            Container container;
            if (!moduleItem.TryGetDataModule<ContainerModuleData>(out var data))
            {
                var containerResult = _containerController.CreateContainer(config.Cols * config.Rows);
                if (!containerResult.HasValue)
                {
                    HLogger.LogError("Cant create container");
                    return Optional<IModuleItem>.Fail();
                }
                
                container = containerResult.Value;

                data = new ContainerModuleData(container.Guid);
                moduleItem.AddDataModule(data);
            }
            else
            {
                var containerResult = _containerController.GetContainer(data.ContainerGuid);
                if (!containerResult.HasValue)
                {
                    HLogger.LogError("Cant get container");
                    return Optional<IModuleItem>.Fail();
                }

                container = containerResult.Value;
            }
            
            _modules.Add(moduleItem.ReferenceSelf, new ContainerModule(moduleItem, data, config, container));
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
            var module = _modules[moduleItem.ReferenceSelf];
            _containerController.DestroyContainer(module.Container);
            _modules.Remove(moduleItem.ReferenceSelf);
        }
    }
}