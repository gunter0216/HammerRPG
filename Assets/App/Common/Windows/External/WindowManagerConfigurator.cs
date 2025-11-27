using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.Windows.External
{
    [Configurator(DIContext.CoreContext)]
    public class WindowManagerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<WindowManager>();

            RegisterFSM<WindowManager>(FSMStage.CoreInitStage, StageOrders.WindowManager);
        }
    }
}