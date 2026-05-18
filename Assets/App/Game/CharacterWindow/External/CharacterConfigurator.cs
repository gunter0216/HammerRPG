using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.CharacterWindow.External
{
    [Configurator(DIContext.CoreContext)]
    public class CharacterConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CharacterWindowController>();
            BindSingle<OpenCharacterWindowSystem>();
        }
    }
}