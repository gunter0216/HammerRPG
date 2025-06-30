using System.Collections.Generic;

namespace App.Common.Autumn.Runtime.Provider
{
    public interface IServiceProvider
    {
        T GetService<T>() where T : class;
        List<object> GetInterfaces<T>() where T : class;
    }
}