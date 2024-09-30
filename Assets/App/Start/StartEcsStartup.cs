using App.Common.HammerDI.External;
using App.Game;
using App.Game.Contexts;
using App.Game.Player.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Start
{
    public class StartEcsStartup : MonoBehaviour
    {
        // [SerializeField] public StaticData StaticData;
        // [SerializeField] public SceneData SceneData;
    
        private EcsWorld m_World;
        private EcsSystems m_UpdateSystems;

        void Start()
        {
            var diManager = DiManager.Instance;
            var serviceProvider = diManager.BuildServiceProvider(typeof(StartSceneContext));
            foreach (IInitSystem initSystem in serviceProvider.GetInterfaces<IInitSystem>())
            {
                initSystem.Init();
            }
            
            Debug.LogError("-----");

            SceneManager.LoadScene("GameScene");

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
    }
}