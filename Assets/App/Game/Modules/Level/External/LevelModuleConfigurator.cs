using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Level.Runtime;
using App.Game.Modules.Level.Runtime.Config;
using App.Game.Modules.Level.Runtime.Data;

namespace App.Game.Modules.Level.External
{
    [Configurator(DIContext.CoreContext)]
    public class LevelModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalLevelModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<LevelModuleDtoToConfigConverter>();
            
            RegisterData<LevelContainerData>();
        }
    }
}