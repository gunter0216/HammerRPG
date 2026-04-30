using App.Common.Configs.Runtime;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.Configs.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class ConfigLoaderConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<ConfigLoader>().AsSingle();
        }
    }
}