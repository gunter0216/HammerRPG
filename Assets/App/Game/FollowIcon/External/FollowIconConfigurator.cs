using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.FollowIcon.External
{
    [Configurator(DIContext.CoreContext)]
    public class FollowIconConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<FollowIconController>();

            RegisterFSM<FollowIconController>(FSMStage.CoreInitStage, StageOrders.FollowIcon);
            
            RegisterUpdate<FollowIconController>(UpdateStage.FollowIcon);
        }
    }
}