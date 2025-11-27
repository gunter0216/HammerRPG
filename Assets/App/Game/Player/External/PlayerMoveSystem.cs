using App.Common.Utilities.Utility.Runtime;
using App.Game.EcsWorlds.Runtime;
using App.Game.Inputs.Runtime.Events;
using App.Game.Player.Runtime.Components;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace App.Game.Player.External
{
    public class PlayerMoveSystem : IInitSystem, IRunSystem
    {
        private readonly IWorldManager m_WorldManager;
        
        private EcsFilter m_EntitiesFilter;
        private EcsPool<EntityComponent> m_EntitiesPool;
        
        private EcsFilter m_AxisFilter;
        private EcsPool<AxisRawEvent> m_AxisPool;

        public PlayerMoveSystem(IWorldManager worldManager)
        {
            m_WorldManager = worldManager;
        }

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

                Debug.LogError($"Horizontal {axis.Horizontal} Vertical {axis.Vertical}");
                var direction = new Vector2(axis.Horizontal, axis.Vertical).normalized;
                var velocity = direction * player.MoveSpeed;
                
                if (player.View == null || player.View.PlayerRigidbody == null)
                {
                    Debug.LogError("view or rigid null");
                    continue;
                }

                player.View.PlayerRigidbody.velocity = velocity;
            }
        }
    }
}