using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Utility.Runtime.Extensions;
using App.Game.Contexts;
using App.Game.EcsWorlds.Runtime;
using App.Game.Inputs.Runtime.Events;
using App.Game.Player.Runtime.Components;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace App.Game.CameraFollow.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(100)]
    public class CameraFollowController : IInitSystem, IRunSystem
    {
        [Inject] private IWorldManager m_WorldManager;
        
        private EcsFilter m_PlayersFilter;
        private EcsPool<EntityComponent> m_EntitiesPool;
        private Camera m_Camera;

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