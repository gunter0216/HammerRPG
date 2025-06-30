using System;
using System.Collections.Generic;
using IServiceProvider = App.Common.Autumn.Runtime.Provider.IServiceProvider;

namespace App.Common.Autumn.Runtime.Collection
{
    public interface IServiceCollection
    {
        void AddTransient(Type type);
        void AddScoped(Type type, object context);
        void AddSingleton(Type type);
        void AddConfigurator(Type configurator);
        void UnloadContext(object context);
        IServiceProvider BuildServiceProvider(object context, List<object> additional);
    }
}