// using App.Common.Timer.Runtime;
// using App.Common.Utilities.Utility.Runtime;
// using App.Game.EcsEvent.Runtime;
// using App.Game.Player.Runtime.Components;
// using App.Game.Player.Runtime.Events;
// using App.Game.Worlds.Runtime;
// using Leopotam.EcsLite;
//
// namespace App.Game.Player.External
// {
//     public class AttackSystem : IInitSystem, IUpdateSystem
//     {
//         private readonly IEcsEventManager m_EcsEventManager;
//         private readonly IWorldManager m_WorldManager;
//         private readonly ITimeManager m_TimeManager;
//         
//         private EcsEventPool<AttackEvent> m_AttackEventPool;
//         private EcsFilter m_AttackEventFilter;
//         private EcsPool<EntityComponent> m_EntitiesPool;
//         private EcsEventPool<PlayAttackAnimationEvent> m_PlayAttackAnimationPool;
//
//         public AttackSystem(IEcsEventManager ecsEventManager, IWorldManager worldManager, ITimeManager timeManager)
//         {
//             m_EcsEventManager = ecsEventManager;
//             m_WorldManager = worldManager;
//             m_TimeManager = timeManager;
//         }
//
//         public void Init()
//         {
//             m_PlayAttackAnimationPool = m_EcsEventManager.GetPool<PlayAttackAnimationEvent>();
//             m_EntitiesPool = m_WorldManager.GetPool<EntityComponent>();
//             
//             m_AttackEventPool = m_EcsEventManager.GetPool<AttackEvent>();
//             m_AttackEventFilter = m_EcsEventManager.GetFilter<AttackEvent>();
//         }
//
//         public void OnUpdate()
//         {
//             return;
//             foreach (var i in m_AttackEventFilter)
//             {
//                 ref var attackEvent = ref m_AttackEventPool.Get(i);
//                 ref var entity = ref m_EntitiesPool.Get(attackEvent.EntityId);
//
//                 if (entity.AttackTimer != null && !entity.AttackTimer.IsCompleted())
//                 {
//                     continue;
//                 }
//                 
//                 m_PlayAttackAnimationPool.Trigger(new PlayAttackAnimationEvent(
//                     entityId: attackEvent.EntityId,
//                     position: attackEvent.Position));
//
//                 entity.AttackTimer = m_TimeManager.CreateRealtimeTimer(1);
//             }
//         }
//     }
// }