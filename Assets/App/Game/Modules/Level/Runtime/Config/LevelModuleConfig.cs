using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Level.Runtime.Config
{
    public class LevelModuleConfig : IModuleConfig
    {
        private readonly int _startLevel;

        public int StartLevel => _startLevel;

        public LevelModuleConfig(int startLevel)
        {
            _startLevel = startLevel;
        }
    }
}