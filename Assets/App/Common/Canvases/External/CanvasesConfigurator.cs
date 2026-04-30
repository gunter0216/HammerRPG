using App.Common.Canvases.External;
using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.ApplicationQuit.External
{
    [Configurator(DIContext.CoreContext)]
    public class CoreCanvasesConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CanvasController>();
            
            FsmRegistrar.Register<CanvasController>(FSMStage.CoreInitStage, StageOrders.CanvasController);
        }
    }
    
    [Configurator(DIContext.MenuContext)]
    public class MenuCanvasesConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CanvasController>();
            
            FsmRegistrar.Register<CanvasController>(FSMStage.MenuInitStage, StageOrders.CanvasController);
        }
    }
    
    [Configurator(DIContext.StartContext)]
    public class StartCanvasesConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CanvasController>();
            
            FsmRegistrar.Register<CanvasController>(FSMStage.StartInitStage, StageOrders.CanvasController);
        }
    }
}