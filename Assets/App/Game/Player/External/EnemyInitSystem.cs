using App.Common.AssetSystem.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.External.View;
using App.Game.Player.Runtime.Components;
using App.Game.Player.Runtime.Events;
using App.Game.States.Game;
using App.Game.Worlds.Runtime;
using UnityEngine;

namespace App.Game.Player.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    sealed class EnemyInitSystem : IInitSystem
    {
        private const string m_EnemyAssetKey = "Enemy";
        private const float m_DefaultMoveSpeed = 5.0f;
        
        [Inject] private IWorldManager m_WorldManager;
        [Inject] private IAssetManager m_AssetManager;
        [Inject] private IEcsEventManager m_EcsEventManager;
        
        private EcsEventPool<WeaponCollisionEvent> m_WeaponCollisionEventPool;

        public void Init()
        {
            var entityView = m_AssetManager.InstantiateSync<EntityView>(new StringKeyEvaluator(m_EnemyAssetKey));
            if (!entityView.HasValue)
            {
                HLogger.LogError("cant create player");
                return;
            }
            
            m_WeaponCollisionEventPool = m_EcsEventManager.GetPool<WeaponCollisionEvent>();
            
            var world = m_WorldManager.GetWorld();
            var enemyEntity = world.NewEntity();
            
            Debug.LogError($"enemy {enemyEntity}");

            var entities = world.GetPool<EntityComponent>();
            var healthPool = world.GetPool<HealthComponent>();
            
            ref var entityComponent = ref entities.Add(enemyEntity);
            ref var healthComponent = ref healthPool.Add(enemyEntity);
            
            healthComponent.Current = 100;
            healthComponent.Max = 100;
            
            entityComponent.View = entityView.Value;
            entityComponent.View.Weapon.gameObject.SetActive(false);
            entityComponent.MoveSpeed = m_DefaultMoveSpeed;
            entityComponent.View.Entity = enemyEntity;

            var weaponView = entityComponent.View.Weapon.GetComponent<WeaponView>();
            if (weaponView != null)
            {
                weaponView.SetOnTriggerEnter2D((other) =>
                {
                    if (other.TryGetComponent<EntityView>(out var attackedEntityView))
                    {
                        m_WeaponCollisionEventPool.Trigger(new WeaponCollisionEvent(
                            attackerEntityId: enemyEntity,
                            attackedEntityId: attackedEntityView.Entity,
                            other: other));
                    }
                });
            }
        }
    }
}