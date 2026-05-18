// using App.Common.Utilities.Utility.Runtime;
// using App.Game.EcsEvent.Runtime;
// using App.Game.Player.Runtime.Components;
// using App.Game.Player.Runtime.Events;
// using App.Game.Worlds.Runtime;
// using Leopotam.EcsLite;
// using Leopotam.EcsLite.Di;
// using UnityEngine;
//
// namespace App.Game.Player.External
// {
//     public class PlayerAttackSystem : IInitSystem, IUpdateSystem
//     {
//         private readonly IEcsEventManager m_EcsEventManager;
//         private readonly IWorldManager m_WorldManager;
//         
//         private EcsEventPool<AttackEvent> m_AttackEventPool;
//         private EcsFilter m_PlayerFilter;
//         private Camera m_Camera;
//
//         public PlayerAttackSystem(IEcsEventManager ecsEventManager, IWorldManager worldManager)
//         {
//             m_EcsEventManager = ecsEventManager;
//             m_WorldManager = worldManager;
//         }
//
//         public void Init()
//         {
//             m_AttackEventPool = m_EcsEventManager.GetPool<AttackEvent>();
//             m_PlayerFilter = m_WorldManager.GetFilter<Inc<EntityComponent, PlayerComponent>>();
//             m_Camera = Camera.main;
//         }
//
//         public void OnUpdate()
//         {
//             if (Input.GetMouseButtonDown(0))
//             {
//                 var mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition); 
//                 var entities = m_PlayerFilter.GetRawEntities();
//                 if (entities.Length > 0)
//                 {
//                     m_AttackEventPool.Trigger(new AttackEvent(
//                         entityId: entities[0],
//                         position: mousePosition));
//                 }
//             }
//         }
//     }
// }