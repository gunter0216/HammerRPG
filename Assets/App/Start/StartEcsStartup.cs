using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.FSM.Runtime;
using App.Common.HammerDI.External;
using App.Common.SceneControllers.External;
using App.Common.SceneControllers.Runtime;
using App.Game;
using App.Game.Contexts;
using App.Game.Player.Systems;
using App.Game.States.Start;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;
using IServiceProvider = App.Common.HammerDI.Runtime.Interfaces.IServiceProvider;

namespace App.Start
{
    public class StartEcsStartup : MonoBehaviour
    {
        // [SerializeField] public StaticData StaticData;
        // [SerializeField] public SceneData SceneData;
    
        private EcsWorld m_World;
        private EcsSystems m_UpdateSystems;
        private IServiceProvider m_ServiceProvider;

        void Start()
        {
            var diManager = DiManager.Instance;
            m_ServiceProvider = diManager.BuildServiceProvider(typeof(StartSceneContext));

            var stateMachine = new StateMachine(m_ServiceProvider.GetInterfaces<IInitSystem>().Cast<IInitSystem>().ToList());
            stateMachine.AddState(new DefaultStage(typeof(StartInitPhase)));
            stateMachine.SyncRun();
            
            var sceneController = m_ServiceProvider.GetService<SceneController>();
            sceneController.LoadScene(SceneConstants.MenuScene);

//             m_World = new EcsWorld();
//             m_UpdateSystems = new EcsSystems(m_World, "MainSystem");
//             var runtimeData = new RuntimeData();
//             
//             m_UpdateSystems
//                 .Add(new PlayerInitSystem())
//                 .Add(new PlayerInputSystem())
//                 .Add(new PlayerMoveSystem())
// #if UNITY_EDITOR
//                 // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
//                 // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
//                 .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
//                 // Регистрируем отладочные системы по контролю за текущей группой систем. 
//                 .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
// #endif
//                 .Inject(StaticData)
//                 .Inject(SceneData)
//                 .Inject(runtimeData)
//                 .Init();
        }

        // void Update()
        // {
        //     m_UpdateSystems?.Run();
        // }
        //
        // void OnDestroy()
        // {
        //     m_UpdateSystems?.Destroy();
        //     m_UpdateSystems = null;
        //     m_World?.Destroy();
        //     m_World = null;
        // }
        
        private void OnDestroy()
        {
            foreach (IDisposable disposable in m_ServiceProvider.GetInterfaces<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}