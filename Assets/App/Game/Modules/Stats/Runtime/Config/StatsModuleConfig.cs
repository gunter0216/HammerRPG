using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Stats.Runtime.Config
{
    public class StatsModuleConfig : IModuleConfig
    {
        private readonly string _startStats;

        public string StartStats => _startStats;

        public StatsModuleConfig(string startStats)
        {
            _startStats = startStats;
        }
    }
}