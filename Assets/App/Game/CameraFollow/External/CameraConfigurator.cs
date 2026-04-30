using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.CameraFollow.External
{
    [Configurator(DIContext.CoreContext)]    
    public class CameraConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CameraFollowController>();
            
            RegisterFSM<CameraFollowController>(FSMStage.CoreInitStage, StageOrders.CameraFollow);
            
            RegisterUpdate<CameraFollowController>(UpdateStage.CameraFollow);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]    
    public class GlobalCameraConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CameraController>();
            
            RegisterFSM<CameraController>(FSMStage.StartInitStage, StageOrders.Camera);
        }
    }
}