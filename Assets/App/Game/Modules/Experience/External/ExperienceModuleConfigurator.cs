using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Experience.Runtime;
using App.Game.Modules.Experience.Runtime.Config;
using App.Game.Modules.Experience.Runtime.Data;

namespace App.Game.Modules.Experience.External
{
    [Configurator(DIContext.CoreContext)]
    public class ExperienceModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalExperienceModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ExperienceModuleDtoToConfigConverter>();
            
            RegisterData<ExperienceContainerData>();
        }
    }
}