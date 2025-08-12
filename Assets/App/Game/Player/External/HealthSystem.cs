using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.Player.Runtime.Events;
using App.Game.States.Runtime.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace App.Game.Player.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(100)]
    public class HealthSystem : IInitSystem, IRunSystem
    {
        [Inject] private IWorldManager m_WorldManager;
        
        private EcsFilter m_HealthFilter;
        private EcsPool<HealthComponent> m_HealthPool;
        private EcsPool<EntityComponent> m_EntityPool;

        public void Init()
        {
            m_HealthPool = m_WorldManager.GetPool<HealthComponent>();
            m_EntityPool = m_WorldManager.GetPool<EntityComponent>();
            m_HealthFilter = m_WorldManager.GetFilter<HealthComponent>();
        }

        public void Run()
        {
            foreach (var i in m_HealthFilter)
            {
                ref var health = ref m_HealthPool.Get(i);
                ref var entity = ref m_EntityPool.Get(i);
                //
                // Debug.LogError($"entity {i} health {health.Current}");
            }
        }
    }
}