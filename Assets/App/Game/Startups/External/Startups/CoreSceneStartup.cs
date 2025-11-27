using App.Common.FSM.External;
using App.Common.FSM.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Core.Startups.External.Constants;
using App.Game.Canvases.External;
using UnityEngine;
using Zenject;

namespace App.Core.Startups.External
{
    public class CoreSceneStartup : MonoInstaller<CoreSceneStartup>
    {
        [SerializeField] private MainCanvas m_MainCanvas;
        [SerializeField] private PopupCanvas m_PopupCanvas;
        
        public override void InstallBindings()
        {
            Container.BindInstance(m_MainCanvas);
            Container.BindInstance(m_PopupCanvas);

            var configuratorsManager = Container.Resolve<ConfiguratorsManager>();
            var fsmRegistrator = Container.Resolve<FSMRegistrar>();
            configuratorsManager.RunConfigurator(DIContext.CoreContext, Container);
            
            var stateMachine = new StateMachine(
                Container.ResolveAll<IInitSystem>(),
                Container.ResolveAll<IPostInitSystem>(),
                fsmRegistrator.GetInfo());
            
            stateMachine.AddState(new DefaultState((int)FSMStage.CoreInitStage));
            stateMachine.SyncRun();
        }
    }
}