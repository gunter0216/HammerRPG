using System;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Chest.Runtime.Config;
using App.Game.Modules.Chest.Runtime.Data;

namespace App.Game.Modules.Chest.Runtime
{
    public class ChestModule
    {
        private readonly IModuleItem _moduleItem;
        private readonly ChestModuleData _data;
        private readonly ChestModuleConfig _config;

        public ChestModule(
            IModuleItem moduleItem, 
            ChestModuleData data, 
            ChestModuleConfig config)
        {
            _moduleItem = moduleItem;
            _data = data;
            _config = config;
        }

        public string IconKey => GetIconKey();
        public bool IsClosed => State == ChestState.Closed;
        public bool IsEmpty => State == ChestState.Empty;
        public bool IsOpen => State == ChestState.Open;
        public ChestState State => _data.State;
        
        public IModuleItem Item => _moduleItem;
        public ChestModuleConfig Config => _config;

        public string GetIconKey()
        {
            if (IsClosed)
            {
                return _config.CloseIconKey;
            }

            if (IsOpen)
            {
                return _config.OpenIconKey;
            }

            if (IsEmpty)
            {
                return _config.EmptyIconKey;
            }

            return String.Empty;
        }
    }
}