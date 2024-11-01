using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.External.Animations;
using App.Game.Player.Runtime;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;

namespace App.Game.Player.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(200)]
    public class PlayAttackAnimationSystem : IInitSystem, IRunSystem
    {
        [Inject] private IEcsEventManager m_EcsEventManager;
        [Inject] private IWorldManager m_WorldManager;

        private EcsFilter m_PlayAttackAnimationEventFilter;
        private EcsEventPool<PlayAttackAnimationEvent> m_PlayAttackAnimationPool;
        private EcsPool<Entity> m_EntitiesPool;

        private EntityMeleeWeaponAnimation m_EntityMeleeWeaponAnimation;

        public void Init()
        {
            m_EntitiesPool = m_WorldManager.GetPool<Entity>();
            
            m_PlayAttackAnimationPool = m_EcsEventManager.GetPool<PlayAttackAnimationEvent>();
            m_PlayAttackAnimationEventFilter = m_EcsEventManager.GetFilter<PlayAttackAnimationEvent>();

            m_EntityMeleeWeaponAnimation = new EntityMeleeWeaponAnimation();
        }

        public void Run()
        {
            foreach (var i in m_PlayAttackAnimationEventFilter)
            {
                ref var attackEvent = ref m_PlayAttackAnimationPool.Get(i);
                ref var entity = ref m_EntitiesPool.Get(attackEvent.EntityId);
                
                m_EntityMeleeWeaponAnimation.StartAnimation(entity.View, attackEvent.Position);
            }
        }
    }
}