using App.Common.Logger.Runtime;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Common.Logger.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class LoggerConfigurator : Core.Startups.External.Configurator
    {
        public override void Configuration()
        {
            Container.Bind<ILogger>().To<Runtime.Logger>().AsSingle();
        }
    }
}