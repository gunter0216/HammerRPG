using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;

namespace App.Common.Logger.External
{
    [Configurator]    
    public class LoggerConfigurator : IConfigurator
    {
        public void Configuration(IConfigurationCollection collection)
        {
            collection.AddSingleton(typeof(Runtime.Logger));
        }
    }
}