namespace App.Common.Time.Runtime
{
    public interface ITimeManager
    {
        RealtimeTimer CreateRealtimeTimer(float startTime);
        void AddRealtimeTimer(RealtimeTimer timer);
    }
}