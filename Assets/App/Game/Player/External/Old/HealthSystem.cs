// using App.Common.Utilities.Utility.Runtime;
// using App.Game.Player.Runtime.Components;
// using App.Game.Worlds.Runtime;
// using Leopotam.EcsLite;
//
// namespace App.Game.Player.External
// {
//     public class HealthSystem : IInitSystem, IUpdateSystem
//     {
//         private readonly IWorldManager m_WorldManager;
//         
//         private EcsFilter m_HealthFilter;
//         private EcsPool<HealthComponent> m_HealthPool;
//         private EcsPool<EntityComponent> m_EntityPool;
//
//         public HealthSystem(IWorldManager worldManager)
//         {
//             m_WorldManager = worldManager;
//         }
//
//         public void Init()
//         {
//             m_HealthPool = m_WorldManager.GetPool<HealthComponent>();
//             m_EntityPool = m_WorldManager.GetPool<EntityComponent>();
//             m_HealthFilter = m_WorldManager.GetFilter<HealthComponent>();
//         }
//
//         public void OnUpdate()
//         {
//             foreach (var i in m_HealthFilter)
//             {
//                 ref var health = ref m_HealthPool.Get(i);
//                 ref var entity = ref m_EntityPool.Get(i);
//                 //
//                 // Debug.LogError($"entity {i} health {health.Current}");
//             }
//         }
//     }
// }