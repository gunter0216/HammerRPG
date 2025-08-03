using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using App.Common.Data.External;

namespace App.Common.ModuleItem.External
{
    [Configurator]
    public class ModuleItemsConfigurator : IConfigurator
    {
        public void Configuration(IConfigurationCollection collection)
        {
            DataManagerProxy.RegisterDataType<ModuleItemContainerData>();
            collection.AddSingleton(typeof(ModuleItemContainerData));
        }
    }
}