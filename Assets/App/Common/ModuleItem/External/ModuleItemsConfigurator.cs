using App.Common.FSM.External;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
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

        public override void OnResolved()
        {
            var moduleItemsManager = Container.Resolve<ModuleItemsManager>();
            var createHandlers = Container.ResolveAll<ICreateModuleItemHandler>();
            var destroyHandlers = Container.ResolveAll<IDestroyModuleItemHandler>();
            moduleItemsManager.AddHandler(createHandlers);
            moduleItemsManager.AddHandler(destroyHandlers);
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