using App.Common.Utilities.Utility.Runtime;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.External.Animations;
using App.Game.Player.Runtime.Components;
using App.Game.Player.Runtime.Events;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;

namespace App.Game.Player.External
{
    public class PlayAttackAnimationSystem : IInitSystem, IUpdateSystem
    {
        private readonly IEcsEventManager m_EcsEventManager;
        private readonly IWorldManager m_WorldManager;

        private EcsFilter m_PlayAttackAnimationEventFilter;
        private EcsEventPool<PlayAttackAnimationEvent> m_PlayAttackAnimationPool;
        private EcsPool<EntityComponent> m_EntitiesPool;

        private EntityMeleeWeaponAnimation m_EntityMeleeWeaponAnimation;

        public PlayAttackAnimationSystem(IEcsEventManager ecsEventManager, IWorldManager worldManager)
        {
            m_EcsEventManager = ecsEventManager;
            m_WorldManager = worldManager;
        }

        public void Init()
        {
            m_EntitiesPool = m_WorldManager.GetPool<EntityComponent>();
            
            m_PlayAttackAnimationPool = m_EcsEventManager.GetPool<PlayAttackAnimationEvent>();
            m_PlayAttackAnimationEventFilter = m_EcsEventManager.GetFilter<PlayAttackAnimationEvent>();

            m_EntityMeleeWeaponAnimation = new EntityMeleeWeaponAnimation();
        }

        public void OnUpdate()
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