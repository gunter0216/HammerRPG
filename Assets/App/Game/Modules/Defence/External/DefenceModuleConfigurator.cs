using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Defence.Runtime;
using App.Game.Modules.Health.Runtime;

namespace App.Game.Modules.Defence.External
{
    [Configurator(DIContext.CoreContext)]
    public class DefenceModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<DefenceModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalDefenceModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
        }
    }
}