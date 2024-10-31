using System;
using Newtonsoft.Json;

namespace App.Common.Time.Runtime
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class RealtimeTimer
    {
        private float m_LeftTime;
        private float m_StartTime;

        public float LeftTime => m_LeftTime;
        public float StartTime => m_StartTime;

        public RealtimeTimer(float startTime)
        {
            m_StartTime = startTime;
            m_LeftTime = startTime;
        }

        public void Decrease(float time)
        {
            m_LeftTime -= time;
        }

        public bool IsCompleted()
        {
            return LeftTime <= 0;
        }
    }
}