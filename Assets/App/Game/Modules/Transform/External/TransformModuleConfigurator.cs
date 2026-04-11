using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Transform.Runtime;
using App.Game.Modules.Transform.Runtime.Config;
using App.Game.Modules.Transform.Runtime.Data;

namespace App.Game.Modules.Transform.External
{
    [Configurator(DIContext.CoreContext)]
    public class TransformModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<TransformModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalTransformModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<TransformModuleDtoToConfigConverter>();
            
            RegisterData<TransformContainerData>();
        }
    }
}