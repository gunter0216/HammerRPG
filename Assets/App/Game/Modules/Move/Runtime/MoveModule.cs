using App.Common.Algorithms.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Move.Runtime.Config;
using App.Game.Modules.Move.Runtime.Data;
using App.Game.Modules.Transform.Runtime.Data;

namespace App.Game.Modules.Move.Runtime
{
    public class MoveModule
    {
        private readonly IModuleItem _moduleItem;
        private readonly MoveModuleData _data;
        private readonly MoveModuleConfig _config;
        
        private Vector2 _direction;

        public MoveModule(
            IModuleItem moduleItem,
            MoveModuleData data,
            MoveModuleConfig config)
        {
            _moduleItem = moduleItem;
            _data = data;
            _config = config;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public Vector2 GetVelocity()
        {
            return _direction * _config.Speed;
        }
    }
}