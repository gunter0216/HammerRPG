using App.Common.Data.Runtime;
using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.Data.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class DataConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<DataSavePathCreator>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataManager>().AsSingle();
            
            FsmRegistrar.Register<DataManager>(FSMStage.StartInitStage, StageOrders.Data);
        }
    }
}