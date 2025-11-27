using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.SceneControllers.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class SceneManagerConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<SceneManager>().AsSingle();
        }
    }
}