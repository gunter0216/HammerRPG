using App.Common.FSM.External;
using App.Common.FSM.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Startups.External.Constants;
using App.Game.Canvases.External;
using UnityEngine;
using Zenject;

namespace App.Core.Startups.External
{
    public class MenuSceneStartup : MonoInstaller<MenuSceneStartup>
    {
        public override void InstallBindings()
        {
            var sceneContext = GetComponent<SceneContext>();
            sceneContext.PostResolve += OnPostResolve;

            var configuratorsManager = Container.Resolve<ConfiguratorsManager>();
            configuratorsManager.RunConfigurator(DIContext.MenuContext, Container);
        }

        private void OnPostResolve()
        {
            var configuratorsManager = Container.Resolve<ConfiguratorsManager>();
            configuratorsManager.OnResolved(DIContext.MenuContext);
            
            var fsmRegistrator = Container.Resolve<FSMRegistrar>();
            
            var stateMachine = new StateMachine(
                Container.ResolveAll<IInitSystem>(),
                Container.ResolveAll<IPostInitSystem>(),
                fsmRegistrator.GetInfo());
            
            stateMachine.AddState(new DefaultState((int)FSMStage.MenuInitStage));
            stateMachine.SyncRun();
        }
    }
}