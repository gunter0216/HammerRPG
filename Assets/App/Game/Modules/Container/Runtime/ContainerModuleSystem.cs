using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Containers.Container.Runtime;
using App.Game.Modules.Container.Runtime.Config;
using App.Game.Modules.Container.Runtime.Data;

namespace App.Game.Modules.Container.Runtime
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

            Containers.Container.Runtime.Container container;
            bool needsData = !moduleItem.TryGetDataModule<ContainerModuleData>(out var data);
            if (config.Cols <= 0 || config.Rows <= 0 || (long)config.Cols * config.Rows > int.MaxValue)
                return Optional<IModuleItem>.Fail();
            if (needsData)
            {
                var containerResult = _containerController.CreateContainer(config.Cols * config.Rows);
                if (!containerResult.HasValue)
                {
                    HLogger.LogError("Cant create container");
                    return Optional<IModuleItem>.Fail();
                }
                
                container = containerResult.Value;
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

            if (!container.TrySetGrid(config.Rows, config.Cols))
            {
                HLogger.LogError("Could not fit the saved items into the container grid.");
                if (needsData)
                    _containerController.DestroyContainer(container);
                return Optional<IModuleItem>.Fail();
            }

            if (needsData)
            {
                data = new ContainerModuleData(container.Guid);
                moduleItem.AddDataModule(data);
            }

            var module = new ContainerModule(moduleItem, data, config, container);
            _modules.Add(moduleItem.ReferenceSelf, module);
            moduleItem.AddModule(module);
            
            return Optional<IModuleItem>.Success(moduleItem);
        }

        public void OnItemDestroyed(IModuleItem moduleItem)
        {
            if (!_modules.TryGetValue(moduleItem.ReferenceSelf, out var module))
                return;
            _containerController.DestroyContainer(module.Container);
            _modules.Remove(moduleItem.ReferenceSelf);
        }

        public ModuleIndex SortIndex()
        {
            return ModuleIndex.Container;
        }
    }
}
