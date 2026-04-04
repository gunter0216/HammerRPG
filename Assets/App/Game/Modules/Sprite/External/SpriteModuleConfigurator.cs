using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.Sprite.Runtime;

namespace App.Game.Modules.Sprite.External
{
    [Configurator(DIContext.GlobalContext)]
    public class SpriteModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<SpriteModuleDtoToConfigConverter>();
        }
    }
}