using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Health.Runtime;
using App.Game.Modules.Health.Runtime.Config;
using App.Game.Modules.Health.Runtime.Data;

namespace App.Game.Modules.Health.External
{
    [Configurator(DIContext.CoreContext)]
    public class HealthModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalHealthModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<HealthModuleDtoToConfigConverter>();
            
            RegisterData<HealthContainerData>();
        }
    }
}