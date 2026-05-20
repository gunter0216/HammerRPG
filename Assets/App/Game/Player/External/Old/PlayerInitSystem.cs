// using App.Common.AssetSystem.Runtime;
// using App.Common.Logger.Runtime;
// using App.Common.Utilities.Utility.Runtime;
// using App.Game.EcsEvent.Runtime;
// using App.Game.Player.External.View;
// using App.Game.Player.Runtime.Components;
// using App.Game.Player.Runtime.Events;
// using App.Game.Worlds.Runtime;
//
// namespace App.Game.Player.External 
// {
//     sealed class PlayerInitSystem : IInitSystem
//     {
//         private const string m_PlayerAssetKey = "Player";
//         private const float m_DefaultMoveSpeed = 10.0f;
//         
//         private readonly IWorldManager m_WorldManager;
//         private readonly IAssetManager m_AssetManager;
//         private readonly IEcsEventManager m_EcsEventManager;
//         
//         private EcsEventPool<WeaponCollisionEvent> m_WeaponCollisionEventPool;
//
//         public PlayerInitSystem(IWorldManager worldManager, IAssetManager assetManager, IEcsEventManager ecsEventManager)
//         {
//             m_WorldManager = worldManager;
//             m_AssetManager = assetManager;
//             m_EcsEventManager = ecsEventManager;
//         }
//
//         public void Init()
//         {
//             var entityView = m_AssetManager.InstantiateSync<EntityView>(new StringKeyEvaluator(m_PlayerAssetKey));
//             if (!entityView.HasValue)
//             {
//                 HLogger.LogError("cant create player");
//                 return;
//             }
//             
//             m_WeaponCollisionEventPool = m_EcsEventManager.GetPool<WeaponCollisionEvent>();
//             
//             var world = m_WorldManager.GetWorld();
//             var playerEntity = world.NewEntity();
//
//             var entities = world.GetPool<EntityComponent>();
//             var playerPool = world.GetPool<PlayerComponent>();
//             var healthPool = world.GetPool<HealthComponent>();
//             
//             ref var entity = ref entities.Add(playerEntity);
//             ref var playerComponent = ref playerPool.Add(playerEntity);
//             ref var healthComponent = ref healthPool.Add(playerEntity);
//             
//             healthComponent.Current = 100;
//             healthComponent.Max = 100;
//             
//             entity.View = entityView.Value;
//             entity.View.Weapon.gameObject.SetActive(false);
//             entity.MoveSpeed = m_DefaultMoveSpeed;
//             entity.View.Entity = playerEntity;
//
//             var weaponView = entity.View.Weapon.GetComponent<WeaponView>();
//             if (weaponView != null)
//             {
//                 weaponView.SetOnTriggerEnter2D((other) =>
//                 {
//                     if (other.TryGetComponent<EntityView>(out var attackedEntityView))
//                     {
//                         m_WeaponCollisionEventPool.Trigger(new WeaponCollisionEvent(
//                             attackerEntityId: playerEntity,
//                             attackedEntityId: attackedEntityView.Entity,
//                             other: other));
//                     }
//                 });
//             }
//         }
//     }
// }
