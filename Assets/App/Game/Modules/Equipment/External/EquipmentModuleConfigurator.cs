using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Equipment.Runtime;
using App.Game.Modules.Equipment.Runtime.Data;

namespace App.Game.Modules.Equipment.External
{
    [Configurator(DIContext.CoreContext)]
    public sealed class EquipmentModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<EquipmentModuleSystem>();
        }
    }

    [Configurator(DIContext.GlobalContext)]
    public sealed class GlobalEquipmentModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<EquipmentModuleContainerData>();
        }
    }
}
