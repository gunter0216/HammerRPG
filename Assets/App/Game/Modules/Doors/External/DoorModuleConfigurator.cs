using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Doors.Runtime;
using App.Game.Modules.Doors.Runtime.Config;
using App.Game.Modules.Doors.Runtime.Data;

namespace App.Game.Modules.Doors.External
{
    [Configurator(DIContext.CoreContext)]
    public class DoorModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<DoorModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalDoorModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<DoorModuleDtoToConfigConverter>();
            
            RegisterData<DoorContainerData>();
        }
    }
}