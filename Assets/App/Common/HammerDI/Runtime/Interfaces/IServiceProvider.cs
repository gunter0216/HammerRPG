using System;
using System.Collections.Generic;

namespace App.Common.HammerDI.Runtime.Interfaces
{
    public interface IServiceProvider
    {
        T GetService<T>() where T : class;
        List<object> GetInterfaces<T>() where T : class;
    }
}