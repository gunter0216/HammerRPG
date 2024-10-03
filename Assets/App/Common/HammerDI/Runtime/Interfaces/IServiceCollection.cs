using System;
using System.Collections.Generic;

namespace App.Common.HammerDI.Runtime.Interfaces
{
    public interface IServiceCollection
    {
        void AddTransient(Type type);
        void AddScoped(Type type, object context);
        void AddSingleton(Type type);
        void UnloadContext(object context);
        IServiceProvider BuildServiceProvider(object context, List<Type> sceneScopeds);
    }
}