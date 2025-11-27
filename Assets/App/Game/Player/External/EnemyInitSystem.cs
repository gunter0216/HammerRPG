using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.EcsEvent.Runtime;
using App.Game.Player.External.View;
using App.Game.Player.Runtime.Components;
using App.Game.Player.Runtime.Events;
using App.Game.Worlds.Runtime;

namespace App.Game.Player.External
{
    sealed class EnemyInitSystem : IInitSystem
    {
        private const string m_EnemyAssetKey = "Enemy";
        private const float m_DefaultMoveSpeed = 5.0f;
        
        private readonly IWorldManager m_WorldManager;
        private readonly IAssetManager m_AssetManager;
        private readonly IEcsEventManager m_EcsEventManager;
        
        private EcsEventPool<WeaponCollisionEvent> m_WeaponCollisionEventPool;

        public EnemyInitSystem(IWorldManager worldManager, IAssetManager assetManager, IEcsEventManager ecsEventManager)
        {
            m_WorldManager = worldManager;
            m_AssetManager = assetManager;
            m_EcsEventManager = ecsEventManager;
        }

        public void Init()
        {
            return;
            var entityView = m_AssetManager.InstantiateSync<EntityView>(new StringKeyEvaluator(m_EnemyAssetKey));
            if (!entityView.HasValue)
            {
                HLogger.LogError("cant create player");
                return;
            }
            
            m_WeaponCollisionEventPool = m_EcsEventManager.GetPool<WeaponCollisionEvent>();
            
            var world = m_WorldManager.GetWorld();
            var enemyEntity = world.NewEntity();

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