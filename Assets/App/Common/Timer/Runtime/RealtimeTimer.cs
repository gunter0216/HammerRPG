using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Common.Timer.Runtime
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class RealtimeTimer : IDisposable
    {
        [SerializeField, JsonProperty("left_time")] private float m_LeftTime;
        [SerializeField, JsonProperty("start_time")] private float m_StartTime;

        private event Action m_OnCompleteAction; 
        private event Action m_OnTickAction; 
        
        public float LeftTime => m_LeftTime;
        public float StartTime => m_StartTime;

        public RealtimeTimer()
        {
            
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init(float startTime)
        {
            m_StartTime = startTime;
            m_LeftTime = startTime;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init(RealtimeTimer other)
        {
            Init(other.StartTime);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetSignals(Action onCompleteAction = null, Action onTickAction = null)
        {
            m_OnCompleteAction = onCompleteAction;
            m_OnTickAction = onTickAction;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ProduceTickSignal()
        {
            m_OnTickAction?.Invoke();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ProduceCompleteSignal()
        {
            m_OnCompleteAction?.Invoke();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Decrease(float time)
        {
            m_LeftTime -= time;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCompleted()
        {
            return LeftTime <= 0;
        }

        public void Dispose()
        {
            m_OnCompleteAction = null;
            m_OnTickAction = null;
        }
    }
}