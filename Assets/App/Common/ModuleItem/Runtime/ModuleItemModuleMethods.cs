using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public partial class ModuleItem
    {
        public bool AddModule<T>(T module) where T : class, IModule
        {
            if (HasModule<T>())
            {
                HLogger.LogError("Модуль уже есть на предмете.");
                return false;
            }
            
            _modules ??= new List<IModule>();
            _modules.Add(module);
            
            return true;
        }

        public bool RemoveModule(IModule module)
        {
            if (_modules == null)
            {
                return false;
            }
            
            return _modules.Remove(module);
        }

        public Optional<T> GetModule<T>() where T : class, IModule
        {
            if (_modules == null)
            {
                return Optional<T>.Fail();
            }

            var module = _modules.FirstOrDefault(x => x is T);
            if (module == default)
            {
                return Optional<T>.Fail();
            }
            
            return Optional<T>.Success(module as T);
        }

        public bool TryGetModule<T>(out T module) where T : class, IModule
        {
            if (_modules == null)
            {
                module = null;
                return false;
            }

            var findModule = _modules.FirstOrDefault(x => x is T);
            if (findModule == default)
            {
                module = null;
                return false;
            }

            module = findModule as T;
            
            return true;
        }

        public bool HasModule<T>() where T : class, IModule
        {
            if (_modules == null)
            {
                return false;
            }

            var module = _modules.FirstOrDefault(x => x is T);
            
            return module != default;
        }
    }
}