using Leopotam.EcsLite;

namespace App.Game.EcsEvent.Runtime
{
    public class EcsEventPool<T> : IEcsEventPool where T : struct
    {
        private readonly EcsPool<T> m_Pool;
        private readonly EcsWorld m_World;

        public EcsEventPool(EcsWorld world)
        {
            m_World = world;
            m_Pool = world.GetPool<T>();
        }
        
        public void Trigger(T eventData)
        {
            var entity = m_World.NewEntity();
            ref T data = ref m_Pool.Add(entity);
            data = eventData;
        }
    }
}