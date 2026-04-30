using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Move.Runtime;
using App.Game.Modules.Move.Runtime.Config;
using App.Game.Modules.Move.Runtime.Data;

namespace App.Game.Modules.Move.External
{
    [Configurator(DIContext.CoreContext)]
    public class MoveModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<MoveModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalMoveModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<MoveContainerData>();
            
            BindSingle<MoveModuleDtoToConfigConverter>();
        }
    }
}