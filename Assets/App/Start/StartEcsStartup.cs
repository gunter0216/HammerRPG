using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.AssemblyManager.External;
using App.Common.AssemblyManager.Runtime;
using App.Common.Autumn.External;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.External;
using App.Common.Data.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.Logger.External;
using App.Common.Logger.Runtime;
using App.Common.SceneControllers.External;
using App.Common.SceneControllers.Runtime;
using App.Common.Utility.Runtime.Extensions;
using App.Game;
using App.Game.Contexts;
using App.Game.States.Start;
using UnityEngine;
using IServiceProvider = App.Common.Autumn.Runtime.Provider.IServiceProvider;
using SceneManager = App.Common.SceneControllers.External.SceneManager;

namespace App.Start
{
    public class StartEcsStartup : MonoBehaviour
    {
        private IServiceProvider m_ServiceProvider;

        void Start()
        {
            HLogger.SetInstance(new UnityLogger());    
            
            var assemblyProvider = new AssemblyManager()
                .CreateAssemblyProviderBuilder()
                .AddAttribute<TransientAttribute>()
                .AddAttribute<SingletonAttribute>()
                .AddAttribute<ScopedAttribute>()
                .AddAttribute<DataAttribute>()
                .AddAttribute<ConfiguratorAttribute>()
                .Build();

            var singletons = assemblyProvider.GetTypes<SingletonAttribute>();
            var scopeds = assemblyProvider.GetTypes<ScopedAttribute>();
            var datas = assemblyProvider.GetTypes<DataAttribute>();
            var configurators = assemblyProvider.GetTypes<ConfiguratorAttribute>();
            var transients = assemblyProvider.GetTypes<TransientAttribute>();

            var diManager = DiManager.Instance;
            diManager.Init(singletons, scopeds, transients, configurators);
            m_ServiceProvider = diManager.BuildServiceProvider(typeof(StartSceneContext));

            m_ServiceProvider.GetService<DataManagerProxy>().SetDatas(datas);

            var stateMachine = new StateMachine(
                m_ServiceProvider.GetInterfaces<IInitSystem>().Cast<IInitSystem>().ToList(),
                m_ServiceProvider.GetInterfaces<IPostInitSystem>().Cast<IPostInitSystem>().ToList());
            stateMachine.AddState(new DefaultStage(typeof(StartInitPhase)));
            stateMachine.SyncRun();

            var sceneController = m_ServiceProvider.GetService<SceneManager>();
            sceneController.LoadScene(SceneConstants.MenuScene);
        }

        private void OnDestroy()
        {
            var dInterfaces = m_ServiceProvider.GetInterfaces<IDisposable>();
            if (dInterfaces.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (IDisposable disposable in dInterfaces)
            {
                disposable.Dispose();
            }
        }
    }
}