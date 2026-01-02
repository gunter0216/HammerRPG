using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.DragItem.External
{
    [Configurator(DIContext.CoreContext)]
    public class DragItemConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<DragItemController>();

            RegisterFSM<DragItemController>(FSMStage.CoreInitStage, StageOrders.DragItem);

            // RegisterUpdate<CameraFollowController>(UpdateStage.CameraFollow);
        }
    }
}