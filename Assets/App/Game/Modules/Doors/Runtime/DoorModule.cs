using System;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Doors.Runtime.Config;
using App.Game.Modules.Doors.Runtime.Data;

namespace App.Game.Modules.Doors.Runtime
{
    public class DoorModule : IModule
    {
        private readonly IModuleItem _moduleItem;
        private readonly DoorModuleData _data;
        private readonly DoorModuleConfig _config;

        public DoorModule(
            IModuleItem moduleItem, 
            DoorModuleData data, 
            DoorModuleConfig config)
        {
            _moduleItem = moduleItem;
            _data = data;
            _config = config;
        }

        public bool IsClosed => State == DoorState.Closed;
        public bool IsOpen => State == DoorState.Open;
        public DoorState State => _data.State;
        
        public IModuleItem Item => _moduleItem;
        public DoorModuleConfig Config => _config;
        public DataReference RequiredKey => _data.Key;

        public void SetKey(DataReference dataReference)
        {
            _data.Key = dataReference;
        }

        public void Close()
        {
            _data.State = DoorState.Closed;
        }

        public void Open()
        {
            _data.State = DoorState.Open;
        }
    }
}