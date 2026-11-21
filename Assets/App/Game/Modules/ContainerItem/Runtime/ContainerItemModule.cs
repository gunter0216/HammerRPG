using App.Common.ModuleItem.Runtime;
using App.Game.Modules.ContainerItem.Runtime.Config;

namespace App.Game.Modules.ContainerItem.Runtime
{
    public class ContainerItemModule : IModule
    {
        private readonly ContainerItemModuleConfig _config;

        public int Width => _config.Width;
        public int Height => _config.Height;

        public ContainerItemModule(ContainerItemModuleConfig config)
        {
            _config = config;
        }
    }
}
