using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.Pause.External
{
    [Configurator(DIContext.CoreContext)]
    public class PauseConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<PauseController>();
        }
    }
}