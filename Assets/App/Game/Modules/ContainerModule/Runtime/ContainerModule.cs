using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Containers.Container.Runtime;
using App.Game.Modules.ContainerModule.Runtime.Config;
using App.Game.Modules.ContainerModule.Runtime.Data;

namespace App.Game.Modules.ContainerModule.Runtime
{
    public class ContainerModule : IModule
    {
        private readonly IModuleItem _item;
        private readonly ContainerModuleData _data;
        private readonly ContainerModuleConfig _config;
        private readonly Container _container;

        // todo make internal
        public Container Container => _container;

        public ContainerModule(
            IModuleItem item, 
            ContainerModuleData data, 
            ContainerModuleConfig config,
            Container container)
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

        // todo здесь должна быть крутая логика со слотами
        public void AddItem(IModuleItem moduleItem)
        {
            for (int i = 0; i < _container.Items.Count; ++i)
            {
                var item = _container.Items[i];
                if (item != null)
                {
                    continue;
                }
                
                _container.AddItem(moduleItem, i);
                
                return;
            }
            
            HLogger.LogError($"Cant add item = {moduleItem.Id}. Not found empty slot.");
        }
    }
}