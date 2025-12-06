using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Common.Events.Runtime
{
    internal interface IHandlerGroup
    {
        public void Clear();
    }
    
    internal sealed class HandlerGroup<T> : IHandlerGroup where T : struct
    {
        private readonly HashSet<Action<T>> _handlers = new();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T message)
        {
            foreach (var action in _handlers)
            {
                action.Invoke(message);
            }
        }
        
        public void Add(Action<T> action)
        {
            _handlers.Add(action);
        }

        public void Remove(Action<T> action)
        {
            _handlers.Remove(action);
        }
        
        public void Clear() => _handlers.Clear();
    }
    
    internal static class EventId<T>
    {
        public static readonly int Id = typeof(T).GetHashCode();
    }
    
    public class EventManager
    {
        private static readonly Dictionary<int, IHandlerGroup> _groups = new();
        private static readonly object _lock = new();
        
        public static void Trigger<T>(T message = default) where T : struct
        {
            lock (_lock)
            {
                if (!_groups.TryGetValue(EventId<T>.Id, out var group))
                    return;

                if (group is not HandlerGroup<T> g)
                    throw new Exception($"Handler not found for type: {typeof(T).Name}");

                g.Invoke(message);
            }
        }
        
        public static void Subscribe<T>(Action<T> action) where T : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            lock (_lock)
            {
                var id = EventId<T>.Id;

                if (!_groups.ContainsKey(id))
                    _groups[id] = new HandlerGroup<T>();

                (_groups[id] as HandlerGroup<T>)?.Add(action);
            }
        }
        
        public static void Unsubscribe<T>(Action<T> action) where T : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (_lock)
            {
                var id = EventId<T>.Id;

                if (!_groups.ContainsKey(id))
                    _groups[id] = new HandlerGroup<T>();

                (_groups[id] as HandlerGroup<T>)?.Remove(action);
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                foreach (var group in _groups.Values)
                    group.Clear();
                
                _groups.Clear();
            }
        }
    }
}