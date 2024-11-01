using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Inputs.Runtime;
using App.Game.Inputs.Runtime.Events;
using App.Game.Player.Runtime;
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
    public class PlayerAttackSystem : IInitSystem, IRunSystem
    {
        [Inject] private IEcsEventManager m_EcsEventManager;
        [Inject] private IWorldManager m_WorldManager;
        
        private EcsEventPool<AttackEvent> m_AttackEventPool;
        private EcsFilter m_PlayerFilter;
        private Camera m_Camera;

        public void Init()
        {
            m_AttackEventPool = m_EcsEventManager.GetPool<AttackEvent>();
            m_PlayerFilter = m_WorldManager.GetFilter<Inc<Entity, PlayerComponent>>();
            m_Camera = Camera.main;
        }

        public void Run()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition); 
                var entities = m_PlayerFilter.GetRawEntities();
                if (entities.Length > 0)
                {
                    m_AttackEventPool.Trigger(new AttackEvent(
                        entityId: entities[0],
                        position: mousePosition));
                }
            }
        }
    }
}