using App.Common.ModuleItem.Runtime;

namespace App.Game.ContainerModule.Runtime
{
    public class ContainerModule
    {
        private readonly IModuleItem _item;
        private readonly ContainerModuleData _data;
        private readonly ContainerModuleConfig _config;
        private readonly Container.Runtime.Container _container;

        internal Container.Runtime.Container Container => _container;

        public ContainerModule(
            IModuleItem item, 
            ContainerModuleData data, 
            ContainerModuleConfig config,
            Container.Runtime.Container container)
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

        public void AddItem(IModuleItem moduleItem)
        {
            for (int i = 0; i < _container.Items.Count; ++i)
            {
                var item = _container.Items[i];
                if (item == null)
                {
                    continue;
                }
                
                _container.AddItem(moduleItem, i);
                
                return;
            }
        }
    }
}