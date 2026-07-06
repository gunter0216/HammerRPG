using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Move.Runtime.Config
{
    public class MoveModuleConfig : ModuleConfig
    {
        private readonly float _speed;

        public float Speed => _speed;

        public MoveModuleConfig(float speed)
        {
            _speed = speed;
        }
    }
}