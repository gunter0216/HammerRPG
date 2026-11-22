using App.Common.ModuleItem.Runtime;
using App.Game.Modules.ContainerItem.Runtime.Config;
using App.Game.Modules.ContainerItem.Runtime.Data;

namespace App.Game.Modules.ContainerItem.Runtime
{
    public class ContainerItemModule : IModule
    {
        private readonly ContainerItemModuleConfig _config;
        private readonly ContainerItemModuleData _data;

        public int Width => _config.Width;
        public int Height => _config.Height;
        public int Index => _data.Index;

        public ContainerItemModule(ContainerItemModuleConfig config, ContainerItemModuleData data)
        {
            _config = config;
            _data = data;
        }
    }
}
