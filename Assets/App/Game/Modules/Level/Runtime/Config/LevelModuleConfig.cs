using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Level.Runtime.Config
{
    public class LevelModuleConfig : IModuleConfig
    {
        private readonly string _startLevel;

        public string StartLevel => _startLevel;

        public LevelModuleConfig(string startLevel)
        {
            _startLevel = startLevel;
        }
    }
}