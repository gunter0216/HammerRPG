using System;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Timer.Runtime;
using App.Game.Contexts;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;

namespace App.Common.Timer.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), -500)]
    [RunSystem(1_000)]
    public class TimeManager : IRunSystem, IInitSystem, ITimeManager
    {
        private InternalTimeManager m_InternalTimeManager;
        
        public void Init()
        {
            m_InternalTimeManager.Init();
        }

        public void Run()
        {
            m_InternalTimeManager.Run(UnityEngine.Time.deltaTime);
        }

        public RealtimeTimer CreateRealtimeTimer(float duration, Action onCompleteAction = null, Action onTickAction = null)
        {
            return m_InternalTimeManager.CreateRealtimeTimer(duration, onCompleteAction, onTickAction);
        }

        public RealtimeTimer CreateRealtimeTimer(RealtimeTimer other, Action onCompleteAction = null, Action onTickAction = null)
        {
            return m_InternalTimeManager.CreateRealtimeTimer(other, onCompleteAction, onTickAction);
        }
    }
}