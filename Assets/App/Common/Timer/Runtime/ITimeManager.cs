using System;

namespace App.Common.Timer.Runtime
{
    public interface ITimeManager
    {
        RealtimeTimer CreateRealtimeTimer(float startTime, Action onCompleteAction = null, Action onTickAction = null);
        RealtimeTimer CreateRealtimeTimer(RealtimeTimer other, Action onCompleteAction = null, Action onTickAction = null);
    }
}