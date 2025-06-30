using System;

namespace App.Common.Autumn.Runtime.Collection
{
    public interface IConfigurationCollection
    {
        void AddTransient(Type type);
        void AddScoped(Type type, object context);
        void AddSingleton(Type type);
    }
}