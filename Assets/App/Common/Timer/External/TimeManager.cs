using System.Collections.Generic;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Time.Runtime;
using App.Game.Contexts;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;

namespace App.Common.Time.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), -500)]
    [RunSystem(1_000)]
    public class TimeManager : IRunSystem, IInitSystem, ITimeManager
    {
        private List<RealtimeTimer> m_RealtimeTimers;
        
        public void Init()
        {
            m_RealtimeTimers = new();
        }

        public void Run()
        {
            var deltaTime = UnityEngine.Time.deltaTime;
            for (int i = 0; i < m_RealtimeTimers.Count; ++i)
            {
                m_RealtimeTimers[i].Decrease(deltaTime);
            }
        }

        public RealtimeTimer CreateRealtimeTimer(float startTime)
        {
            var timer = new RealtimeTimer(startTime);
            m_RealtimeTimers.Add(timer);
            return timer;
        }
        
        public void AddRealtimeTimer(RealtimeTimer timer)
        {
            m_RealtimeTimers.Add(timer);
        }
    }
}