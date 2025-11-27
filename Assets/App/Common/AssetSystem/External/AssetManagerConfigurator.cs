using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.AssetSystem.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class AssetManagerConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<AssetManager>().AsSingle();
        }
    }
}