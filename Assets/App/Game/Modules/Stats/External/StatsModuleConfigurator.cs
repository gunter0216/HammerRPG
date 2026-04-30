using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Stats.Runtime;
using App.Game.Modules.Stats.Runtime.Config;
using App.Game.Modules.Stats.Runtime.Data;

namespace App.Game.Modules.Stats.External
{
    [Configurator(DIContext.CoreContext)]
    public class StatsModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalStatsModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<StatsModuleDtoToConfigConverter>();
            
            RegisterData<StatsContainerData>();
        }
    }
}