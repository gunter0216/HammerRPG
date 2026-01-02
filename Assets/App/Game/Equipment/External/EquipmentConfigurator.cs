using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.ModuleItemType.Runtime.Config.Converter;
using App.Game.Update.External;

namespace App.Game.Equipment.External
{
    [Configurator(DIContext.CoreContext)]
    public class EquipmentConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<EquipmentController>();

            RegisterFSM<EquipmentController>(FSMStage.CoreInitStage, StageOrders.Equipment);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalEquipmentConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<EquipmentModuleDtoToConfigConverter>();
        }
    }
}