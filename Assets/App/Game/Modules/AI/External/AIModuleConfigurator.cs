using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.AI.Runtime;

namespace App.Game.Modules.AI.External
{
    [Configurator(DIContext.CoreContext)]
    public sealed class AIModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<AIModuleSystem>();
        }
    }
}
