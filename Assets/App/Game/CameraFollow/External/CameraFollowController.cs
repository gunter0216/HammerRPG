using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using App.Game.Player.Runtime.Components;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace App.Game.CameraFollow.External
{
    public class CameraFollowController : IInitSystem, IRunSystem
    {
        private readonly IWorldManager m_WorldManager;
        
        private EcsFilter m_PlayersFilter;
        private EcsPool<EntityComponent> m_EntitiesPool;
        private Camera m_Camera;

        public CameraFollowController(IWorldManager worldManager)
        {
            m_WorldManager = worldManager;
        }

        public void Init()
        {
            m_Camera = Camera.main;
            
            var world = m_WorldManager.GetWorld();
            m_PlayersFilter = world.Filter<PlayerComponent>().End();
            m_EntitiesPool = world.GetPool<EntityComponent>();
        }

        public void Run()
        {
            foreach (var i in m_PlayersFilter)
            {
                ref var player = ref m_EntitiesPool.Get(i);
                var playerPosition = player.View.Transform.position;
                
                m_Camera.transform.SetPositionX(playerPosition.x);
                m_Camera.transform.SetPositionY(playerPosition.y);
            }
        }
    }
}