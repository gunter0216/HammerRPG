using App.Common.AssemblyManager.External;
using App.Common.Data.Runtime;
using App.Common.FSM.External;
using App.Common.Logger.External;
using App.Common.Logger.Runtime;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using Zenject;

namespace App.Core.Startups.External
{
    public class GlobalStartup : MonoInstaller<StartSceneStartup>
    {
        private ConfiguratorsManager _configuratorsManager;

        public override void InstallBindings()
        {
            ProjectContext.PostResolve += OnPostResolve;
            
            HLogger.SetInstance(new UnityLogger());
            
            var assemblyProvider = new AssemblyManager()
                .CreateAssemblyProviderBuilder()
                .AddAttribute<ConfiguratorAttribute>()
                .Build();
            
            var configurators = assemblyProvider.GetTypes<ConfiguratorAttribute>();
            var fsmRegistrar = new FSMRegistrar();
            var dataRegistrar = new DataRegistrar();
            
            _configuratorsManager = new ConfiguratorsManager(configurators, fsmRegistrar, dataRegistrar);
            _configuratorsManager.RunConfigurator(DIContext.GlobalContext, Container);
            
            Container.BindInstance(_configuratorsManager);
            Container.BindInstance(fsmRegistrar);
            Container.BindInstance(dataRegistrar);
        }

        private void OnPostResolve()
        {
            _configuratorsManager.OnResolved(DIContext.GlobalContext);
        }
    }
}