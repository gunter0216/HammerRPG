using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.Update.External
{
    [Configurator(DIContext.CoreContext)]    
    public class UpdateManagerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<UpdateManager>();
            
            RegisterFSM<UpdateManager>(FSMStage.CoreInitStage, StageOrders.UpdateManager);
        }
    }
}