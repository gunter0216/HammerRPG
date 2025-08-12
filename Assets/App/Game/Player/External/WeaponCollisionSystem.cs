using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.External.Animations;
using App.Game.Player.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.Player.Runtime.Events;
using App.Game.States.Runtime.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;
using UnityEngine;

namespace App.Game.Player.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(300)]
    public class WeaponCollisionSystem : IInitSystem, IRunSystem
    {
        [Inject] private IEcsEventManager m_EcsEventManager;
        [Inject] private IWorldManager m_WorldManager;

        private EcsFilter m_WeaponCollisionEventFilter;
        private EcsEventPool<WeaponCollisionEvent> m_WeaponCollisionEventPool;
        private EcsPool<EntityComponent> m_EntitiesPool;
        private EcsPool<HealthComponent> m_HealthPool;

        private EntityMeleeWeaponAnimation m_EntityMeleeWeaponAnimation;

        public void Init()
        {
            m_EntitiesPool = m_WorldManager.GetPool<EntityComponent>();
            m_HealthPool = m_WorldManager.GetPool<HealthComponent>();
            
            m_WeaponCollisionEventPool = m_EcsEventManager.GetPool<WeaponCollisionEvent>();
            m_WeaponCollisionEventFilter = m_EcsEventManager.GetFilter<WeaponCollisionEvent>();
        }

        public void Run()
        {
            foreach (var i in m_WeaponCollisionEventFilter)
            {
                var attackEvent = m_WeaponCollisionEventPool.Get(i);
                if (attackEvent.AttackerEntityId == attackEvent.AttackedEntityId)
                {
                    continue;
                }

                ref var healthAttacked = ref m_HealthPool.Get(attackEvent.AttackedEntityId);
                healthAttacked.Current -= 10;
                
                // ref var attackerEntity = ref m_EntitiesPool.Get(attackEvent.AttackerEntityId);
                // ref var attackedEntity = ref m_EntitiesPool.Get(attackEvent.AttackedEntityId);
                
                // Debug.LogError($"attackerEntity {attackEvent.AttackerEntityId} attackedEntity {attackEvent.AttackedEntityId}");
                
            }
        }
    }
}