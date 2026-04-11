using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Chests.Runtime;
using App.Game.Modules.Chests.Runtime.Config;
using App.Game.Modules.Chests.Runtime.Data;

namespace App.Game.Modules.Chests.External
{
    [Configurator(DIContext.CoreContext)]
    public class ChestModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ChestModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalChestModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ChestModuleDtoToConfigConverter>();
            
            RegisterData<ChestContainerData>();
        }
    }
}