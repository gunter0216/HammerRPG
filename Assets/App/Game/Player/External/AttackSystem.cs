using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Timer.Runtime;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.Player.Runtime.Events;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;

namespace App.Game.Player.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(100)]
    public class AttackSystem : IInitSystem, IRunSystem
    {
        [Inject] private IEcsEventManager m_EcsEventManager;
        [Inject] private IWorldManager m_WorldManager;
        [Inject] private ITimeManager m_TimeManager;
        
        private EcsEventPool<AttackEvent> m_AttackEventPool;
        private EcsFilter m_AttackEventFilter;
        private EcsPool<EntityComponent> m_EntitiesPool;
        private EcsEventPool<PlayAttackAnimationEvent> m_PlayAttackAnimationPool;

        public void Init()
        {
            m_PlayAttackAnimationPool = m_EcsEventManager.GetPool<PlayAttackAnimationEvent>();
            m_EntitiesPool = m_WorldManager.GetPool<EntityComponent>();
            
            m_AttackEventPool = m_EcsEventManager.GetPool<AttackEvent>();
            m_AttackEventFilter = m_EcsEventManager.GetFilter<AttackEvent>();
        }

        public void Run()
        {
            foreach (var i in m_AttackEventFilter)
            {
                ref var attackEvent = ref m_AttackEventPool.Get(i);
                ref var entity = ref m_EntitiesPool.Get(attackEvent.EntityId);

                if (entity.AttackTimer != null && !entity.AttackTimer.IsCompleted())
                {
                    continue;
                }
                
                m_PlayAttackAnimationPool.Trigger(new PlayAttackAnimationEvent(
                    entityId: attackEvent.EntityId,
                    position: attackEvent.Position));

                entity.AttackTimer = m_TimeManager.CreateRealtimeTimer(1);
            }
        }
    }
}