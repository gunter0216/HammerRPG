using System;
using System.Linq;
using App.Common.Autumn.External;
using App.Common.FSM.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.Utility.Runtime.Extensions;
using App.Game;
using App.Game.Contexts;
using App.Game.States.Menu;
using Leopotam.EcsLite;
using UnityEngine;
using IServiceProvider = App.Common.Autumn.Runtime.Provider.IServiceProvider;

namespace App.Menu
{
    public class MenuStartup : MonoBehaviour
    {
        private IServiceProvider m_ServiceProvider;

        private void Start()
        {
            var diManager = DiManager.Instance;
            m_ServiceProvider = diManager.BuildServiceProvider(typeof(MenuSceneContext));

            var stateMachine = new StateMachine(
                m_ServiceProvider.GetInterfaces<IInitSystem>().Cast<IInitSystem>().ToList(),
                m_ServiceProvider.GetInterfaces<IPostInitSystem>().Cast<IPostInitSystem>().ToList());
            stateMachine.AddState(new DefaultStage(typeof(MenuInitPhase)));
            stateMachine.SyncRun();
            
            // todo
            var sceneManager = m_ServiceProvider.GetService<ISceneManager>();
            sceneManager.LoadScene(SceneConstants.GameScene);
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