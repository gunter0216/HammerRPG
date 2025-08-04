using System;
using System.Collections.Generic;
using App.Common.Utility.Pool.Runtime;
using App.Common.Utility.Runtime;

namespace App.Common.Timer.Runtime
{
    public class InternalTimeManager : ITimeManager
    {
        private ListPool<RealtimeTimer> m_RealtimeTimers;

        private List<PoolItemHolder<RealtimeTimer>> m_CompletedTimers;
        
        public void Init()
        {
            m_CompletedTimers = new List<PoolItemHolder<RealtimeTimer>>();
            
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
                timer.Item.Decrease(deltaTime);
            }
            
            for (int i = 0; i < m_RealtimeTimers.ActiveItems.Count; ++i)
            {
                var timer = m_RealtimeTimers.ActiveItems[i];
                timer.Item.ProduceTickSignal();
            }
            
            for (int i = 0; i < m_RealtimeTimers.ActiveItems.Count; ++i)
            {
                var timer = m_RealtimeTimers.ActiveItems[i];
                if (timer.Item.IsCompleted())
                {
                    m_CompletedTimers.Add(timer);
                }
            }

            for (int i = 0; i < m_CompletedTimers.Count; ++i)
            {
                var timer = m_CompletedTimers[i];
                timer.Item.ProduceCompleteSignal();
                m_RealtimeTimers.Release(timer);
            }
            
            m_CompletedTimers.Clear();
        }

        public RealtimeTimer CreateRealtimeTimer(float duration, Action onCompleteAction = null, Action onTickAction = null)
        {
            var timer = m_RealtimeTimers.Get();
            timer.Value.Item.Init(duration);
            timer.Value.Item.SetSignals(onCompleteAction, onTickAction);
            return timer.Value.Item;
        }

        public RealtimeTimer CreateRealtimeTimer(RealtimeTimer other, Action onCompleteAction = null, Action onTickAction = null)
        {
            var timer = m_RealtimeTimers.Get();
            timer.Value.Item.Init(other);
            timer.Value.Item.SetSignals(onCompleteAction, onTickAction);
            return timer.Value.Item;
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