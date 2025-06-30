using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;

namespace App.Common.Data.External
{
    [Configurator]
    public class DataConfigurator : IConfigurator
    {
        public void Configuration(IConfigurationCollection collection)
        {
            // collection.AddSingleton(typeof(DataManagerProxy));
        }
    }
}