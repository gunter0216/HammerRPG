using System;
using UniRx;

namespace App.Common.Events.External
{
    public class EventManager
    {
        public static IDisposable Subscribe<T>(Action<T> callback)
        {
            return MessageBroker.Default.Receive<T>().Subscribe(callback);
        }
        
        public static void Trigger<T>(T value)
        {
            MessageBroker.Default.Publish<T>(value);
        }
    }
}