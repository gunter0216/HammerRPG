using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.SpriteLoaders.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class SpriteLoaderConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.BindInterfacesAndSelfTo<SpriteLoader>().AsSingle();
            Container.Bind<IItemSpriteLoader>().To<ItemSpriteLoader>().AsSingle();
            
            FsmRegistrar.Register<SpriteLoader>(FSMStage.StartInitStage, 0);
        }
    }
    
    [Configurator(DIContext.CoreContext)]    
    public class CoreSpriteLoaderConfigurator : Configurator
    {
        public override void Configuration()
        {
            Container.Bind<IItemSpriteLoader>().To<ItemSpriteLoader>().AsSingle();
        }
    }
}