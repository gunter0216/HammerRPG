using App.Common.ModuleItem.Runtime;
using App.Game.Modules.AI.Runtime.Config;

namespace App.Game.Modules.AI.Runtime
{
    public sealed class AIModule : IModule
    {
        private readonly AIModuleConfig _config;

        public float DetectionRadius => _config.DetectionRadius;
        public float TargetScanInterval => _config.TargetScanInterval;

        public AIModule(AIModuleConfig config)
        {
            _config = config;
        }
    }
}
