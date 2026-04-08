using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Move.Runtime.Config
{
    public class MoveModuleConfig : IModuleConfig
    {
        private readonly string _startMove;

        public string StartMove => _startMove;

        public MoveModuleConfig(string startMove)
        {
            _startMove = startMove;
        }
    }
}