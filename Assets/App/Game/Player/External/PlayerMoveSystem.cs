using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsWorlds.Runtime;
using App.Game.Inputs.Runtime.Events;
using App.Game.Player.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.States.Game;
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
    [RunSystem(0)]
    public class PlayerMoveSystem : IInitSystem, IRunSystem
    {
        [Inject] private IWorldManager m_WorldManager;
        
        private EcsFilter m_EntitiesFilter;
        private EcsPool<EntityComponent> m_EntitiesPool;
        
        private EcsFilter m_AxisFilter;
        private EcsPool<AxisRawEvent> m_AxisPool;

        public void Init()
        {
            var eventWorld = m_WorldManager.GetWorld(WorldConstants.Event);
            m_AxisFilter = eventWorld.Filter<AxisRawEvent>().End();
            m_AxisPool = eventWorld.GetPool<AxisRawEvent>();
            
            var world = m_WorldManager.GetWorld();
            m_EntitiesFilter = world.Filter<EntityComponent>().End();
            m_EntitiesPool = world.GetPool<EntityComponent>();
        }

        public void Run()
        {
            var axis = m_AxisPool.Get(m_AxisFilter.GetRawEntities()[0]);
            
            foreach (var i in m_EntitiesFilter)
            {
                ref var player = ref m_EntitiesPool.Get(i);

                var direction = new Vector2(axis.Horizontal, axis.Vertical).normalized;
                var velocity = direction * player.MoveSpeed;
                player.View.PlayerRigidbody.velocity = velocity;
            }
        }
    }
}