using System;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.EcsWorlds.Runtime;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;

namespace App.Game.Worlds.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), -100_000)]
    [RunSystem(-100_000)]
    public class WorldManager : IInitSystem, IPostInitSystem, IRunSystem, IWorldManager, IDisposable
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

        public void Run()
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