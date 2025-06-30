using System;
using System.Linq;
using App.Common.Autumn.External;
using App.Common.FSM.Runtime;
using App.Common.Utility.Runtime.Extensions;
using App.Game.Contexts;
using App.Game.States.Game;
using App.Game.Update.External;
using UnityEngine;
using IServiceProvider = App.Common.Autumn.Runtime.Provider.IServiceProvider;

namespace App.Game
{
    sealed class GameEcsStartup : MonoBehaviour
    {
        private IServiceProvider m_ServiceProvider;
        private UpdateManager m_UpdateManager;

        void Start()
        {
            var diManager = DiManager.Instance;
            m_ServiceProvider = diManager.BuildServiceProvider(typeof(GameSceneContext));
            
            var stateMachine = new StateMachine(
                m_ServiceProvider.GetInterfaces<IInitSystem>().Cast<IInitSystem>().ToList(),
                m_ServiceProvider.GetInterfaces<IPostInitSystem>().Cast<IPostInitSystem>().ToList());
            stateMachine.AddState(new DefaultStage(typeof(GameInitPhase)));
            stateMachine.SyncRun();

            m_UpdateManager = m_ServiceProvider.GetService<UpdateManager>();
        }

        void Update()
        {
            m_UpdateManager?.Run();
        }

        void OnDestroy()
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