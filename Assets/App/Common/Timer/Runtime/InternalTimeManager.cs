using System;
using System.Collections.Generic;
using App.Common.Utility.Runtime.Pool;

namespace App.Common.Timer.Runtime
{
    public struct InternalTimeManager : ITimeManager
    {
        private ListPool<RealtimeTimer> m_RealtimeTimers;

        private List<RealtimeTimer> m_CompletedTimers;
        
        public void Init()
        {
            m_CompletedTimers = new List<RealtimeTimer>();
            
            m_RealtimeTimers = new ListPool<RealtimeTimer>(
                createFunc: CreateRealtimeTimer,
                actionOnRelease: ReleaseTimer,
                actionOnDestroy: ReleaseTimer);
        }

        public void Run(float deltaTime)
        {
            for (int i = 0; i < m_RealtimeTimers.ActiveItems.Count; ++i)
            {
                var timer = m_RealtimeTimers.ActiveItems[i];
                timer.Decrease(deltaTime);
            }
            
            for (int i = 0; i < m_RealtimeTimers.ActiveItems.Count; ++i)
            {
                var timer = m_RealtimeTimers.ActiveItems[i];
                timer.ProduceTickSignal();
            }
            
            for (int i = 0; i < m_RealtimeTimers.ActiveItems.Count; ++i)
            {
                var timer = m_RealtimeTimers.ActiveItems[i];
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
            }
            
            m_CompletedTimers.Clear();
        }

        public RealtimeTimer CreateRealtimeTimer(float startTime, Action onCompleteAction = null, Action onTickAction = null)
        {
            var timer = m_RealtimeTimers.Get();
            timer.Init(startTime);
            timer.SetSignals(onCompleteAction, onTickAction);
            return timer;
        }

        public RealtimeTimer CreateRealtimeTimer(RealtimeTimer other, Action onCompleteAction = null, Action onTickAction = null)
        {
            var timer = m_RealtimeTimers.Get();
            timer.Init(other);
            timer.SetSignals(onCompleteAction, onTickAction);
            return timer;
        }

        private RealtimeTimer CreateRealtimeTimer()
        {
            return new RealtimeTimer();
        }
        
        private void ReleaseTimer(RealtimeTimer timer)
        {
            timer.Dispose();
        }
    }
}