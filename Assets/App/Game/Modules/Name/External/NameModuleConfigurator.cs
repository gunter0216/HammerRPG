using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Name.Runtime;
using App.Game.Modules.Name.Runtime.Config;
using App.Game.Modules.Name.Runtime.Data;

namespace App.Game.Modules.Name.External
{
    [Configurator(DIContext.CoreContext)]
    public class NameModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalNameModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<NameModuleDtoToConfigConverter>();
            
            RegisterData<NameContainerData>();
        }
    }
}