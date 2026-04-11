using App.Common.Input.Runtime;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.CharacterWindow.External
{
    [Configurator(DIContext.GlobalContext)]
    public class InputConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<InputService>();
        }
    }
}