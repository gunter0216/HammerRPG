// using System;
// using System.Collections.Generic;
// using App.Common.Utilities.Utility.Runtime;
// using App.Game.EcsEvent.Runtime;
// using App.Game.EcsWorlds.Runtime;
// using App.Game.Worlds.Runtime;
// using Leopotam.EcsLite;
// using Leopotam.EcsLite.ExtendedSystems;
//
// namespace App.Game.EcsEvent.External
// {
//     public class EcsEventManager : IInitSystem, IUpdateSystem, IEcsEventManager
//     {
//         private readonly IWorldManager m_WorldManager;
//         
//         private EcsWorld m_World;
//         private Dictionary<Type, IEcsEventPool> m_EventPools;
//         private List<IEcsRunSystem> m_DelSystems;
//         private EcsSystems m_Systems;
//
//         public EcsEventManager(IWorldManager worldManager)
//         {
//             m_WorldManager = worldManager;
//         }
//
//         public void Init()
//         {
//             m_World = m_WorldManager.GetWorld(WorldConstants.Event);
//             m_Systems = m_WorldManager.GetSystems();
//             m_DelSystems = new List<IEcsRunSystem>();
//             m_EventPools = new Dictionary<Type, IEcsEventPool>();
//         }
//
//         public EcsEventPool<T> GetPool<T>() where T : struct
//         {
//             if (m_EventPools.TryGetValue(typeof(T), out var pool))
//             {
//                 return pool as EcsEventPool<T>;
//             }
//             
//             var eventPool = new EcsEventPool<T>(m_World);
//             AddDelSystem<T>();
//             return eventPool;
//         }
//         
//         public EcsFilter GetFilter<T>() where T : struct
//         {
//             return m_World.Filter<T>().End();
//         }
//
//         public void OnUpdate()
//         {
//             for (int i = 0; i < m_DelSystems.Count; ++i)
//             {
//                 m_DelSystems[i].Run(m_Systems);
//             }
//         }
//
//         private void AddDelSystem<T>() where T : struct
//         {
//             m_DelSystems.Add(new DelHereSystem<T>(m_World));
//         }
//     }
// }