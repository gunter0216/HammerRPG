using App.Common.FSM.External;
using App.Common.ModuleItem.Runtime.Data;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.ModuleItem.External
{
    [Configurator(DIContext.CoreContext)]
    public class ModuleItemsConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ModuleItemsManager>();

            RegisterFSM<ModuleItemsManager>(FSMStage.CoreInitStage, StageOrders.ModuleItemsManager);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalModuleItemsConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<ModuleItemContainerData>();
        }
    }
}