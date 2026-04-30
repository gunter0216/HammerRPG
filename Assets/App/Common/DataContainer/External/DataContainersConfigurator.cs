using App.Common.DataContainer.Runtime;
using App.Common.DataContainer.Runtime.Data.Loader;
using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.DataContainer.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class DataContainersConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<ContainerDataLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<ContainersDataManager>().AsSingle();
            
            FsmRegistrar.Register<ContainersDataManager>(FSMStage.StartInitStage, StageOrders.DataContainers);
        }
    }
}