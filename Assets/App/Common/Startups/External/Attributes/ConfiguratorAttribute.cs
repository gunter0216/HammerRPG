using System;
using App.Core.Startups.External.Constants;

namespace App.Core.Startups.External.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ConfiguratorAttribute : Attribute
    {
        public int Context { get; set; }

        public ConfiguratorAttribute(int context)
        {
            Context = context;
        }
        
        public ConfiguratorAttribute(DIContext context)
        {
            Context = (int)context;
        }
    }
}