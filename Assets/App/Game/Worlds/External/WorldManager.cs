using System;
using App.Common.Utilities.Utility.Runtime;
using App.Game.EcsWorlds.Runtime;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;

namespace App.Game.Worlds.External
{
    public class WorldManager : IInitSystem, IPostInitSystem, IUpdateSystem, IWorldManager, IDisposable
    {
        private EcsWorld m_World;
        private EcsSystems m_Systems;

        public void Init()
        {
            m_World = new EcsWorld();
            m_Systems = new EcsSystems(m_World, "MainSystem");

            m_Systems
                .AddWorld(new EcsWorld(), WorldConstants.Event)
#if UNITY_EDITOR
                // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(WorldConstants.Event))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                // Регистрируем отладочные системы по контролю за текущей группой систем. 
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem(WorldConstants.Event))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif
        }

        public void PostInit()
        {
            m_Systems.Init();
        }

        public void OnUpdate()
        {
            m_Systems.Run();
        }

        public EcsSystems GetSystems()
        {
            return m_Systems;
        }
        
        public EcsWorld GetWorld(string name = null)
        {
            return m_Systems.GetWorld(name);
        }
        
        public EcsPool<T> GetPool<T>(string worldName = null) where T : struct
        {
            return m_Systems.GetWorld(worldName).GetPool<T>();
        }
        
        public EcsFilter GetFilter<T>(string worldName = null) where T : struct
        {
            return m_Systems.GetWorld(worldName).Filter<T>().End();
        }

        public void Dispose()
        {
            m_Systems?.Destroy();
            m_Systems = null;
            m_World?.Destroy();
            m_World = null;
        }
    }
}