using System;

namespace App.Common.Autumn.Runtime.Collection
{
    public class ConfigurationCollection : IConfigurationCollection
    {
        private readonly IServiceCollection m_ServiceCollection;

        public ConfigurationCollection(IServiceCollection serviceCollection)
        {
            m_ServiceCollection = serviceCollection;
        }

        public void AddTransient(Type type)
        {
            m_ServiceCollection.AddTransient(type);
        }

        public void AddScoped(Type type, object context)
        {
            m_ServiceCollection.AddScoped(type, context);
        }

        public void AddSingleton(Type type)
        {
            m_ServiceCollection.AddSingleton(type);
        }
    }
}