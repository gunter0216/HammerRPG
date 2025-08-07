using System;
using System.Collections.Generic;
using App.Common.Utility.Pool.Runtime;
using App.Common.Utility.Runtime;

namespace App.Common.Timer.Runtime
{
    public class InternalTimeManager : ITimeManager
    {
        private ListPool<RealtimeTimer> m_RealtimeTimers;

        private List<RealtimeTimer> m_CompletedTimers;
        private List<RealtimeTimer> m_ActiveTimers;
        
        public void Init()
        {
            m_CompletedTimers = new List<RealtimeTimer>();
            m_ActiveTimers = new List<RealtimeTimer>();
            
            m_RealtimeTimers = new ListPool<RealtimeTimer>(
                createFunc: CreateRealtimeTimer,
                actionOnRelease: ReleaseTimer,
                actionOnDestroy: ReleaseTimer);
        }

        public void Run(float deltaTime)
        {
            for (int i = 0; i < m_ActiveTimers.Count; ++i)
            {
                var timer = m_ActiveTimers[i];
                timer.Decrease(deltaTime);
            }
            
            for (int i = 0; i < m_ActiveTimers.Count; ++i)
            {
                var timer = m_ActiveTimers[i];
                timer.ProduceTickSignal();
            }
            
            for (int i = 0; i < m_ActiveTimers.Count; ++i)
            {
                var timer = m_ActiveTimers[i];
                if (timer.IsCompleted())
                {
                    m_CompletedTimers.Add(timer);
                }
            }

            for (int i = 0; i < m_CompletedTimers.Count; ++i)
            {
                var timer = m_CompletedTimers[i];
                timer.ProduceCompleteSignal();
                m_RealtimeTimers.Release(timer);
                m_ActiveTimers.Remove(timer); // todo up
            }
            
            m_CompletedTimers.Clear();
        }

        public RealtimeTimer CreateRealtimeTimer(float duration, Action onCompleteAction = null, Action onTickAction = null)
        {
            // todo optional
            var timer = m_RealtimeTimers.Get();
            timer.Value.Init(duration);
            timer.Value.SetSignals(onCompleteAction, onTickAction);
            m_ActiveTimers.Add(timer.Value);
            return timer.Value;
        }

        public RealtimeTimer CreateRealtimeTimer(RealtimeTimer other, Action onCompleteAction = null, Action onTickAction = null)
        {
            // todo optional
            var timer = m_RealtimeTimers.Get();
            timer.Value.Init(other);
            timer.Value.SetSignals(onCompleteAction, onTickAction);
            m_ActiveTimers.Add(timer.Value);
            return timer.Value;
        }

        private Optional<RealtimeTimer> CreateRealtimeTimer()
        {
            return Optional<RealtimeTimer>.Success(new RealtimeTimer());
        }
        
        private void ReleaseTimer(RealtimeTimer timer)
        {
            timer.Dispose();
        }
    }
}