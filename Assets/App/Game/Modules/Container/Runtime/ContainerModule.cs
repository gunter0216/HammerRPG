using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Container.Runtime.Config;
using App.Game.Modules.Container.Runtime.Data;

namespace App.Game.Modules.Container.Runtime
{
    public class ContainerModule : IModule
    {
        private readonly IModuleItem _item;
        private readonly ContainerModuleData _data;
        private readonly ContainerModuleConfig _config;
        private readonly Containers.Container.Runtime.Container _container;

        // todo make internal
        public Containers.Container.Runtime.Container Container => _container;

        public ContainerModule(
            IModuleItem item, 
            ContainerModuleData data, 
            ContainerModuleConfig config,
            Containers.Container.Runtime.Container container)
        {
            _item = item;
            _data = data;
            _config = config;
            _container = container;
        }

        public void Remove(int slotIndex)
        {
            _container.Remove(slotIndex);
        }

        public bool AddItem(IModuleItem moduleItem)
        {
            int index = _container.FindSpace(moduleItem);
            return index >= 0 && _container.AddItem(moduleItem, index);
        }
    }
}
