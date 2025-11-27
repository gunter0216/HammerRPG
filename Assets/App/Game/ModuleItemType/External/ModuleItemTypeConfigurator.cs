using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.ModuleItemType.Runtime.Config.Converter;

namespace App.Game.ModuleItemType.External
{
    [Configurator(DIContext.GlobalContext)]
    public class ModuleItemTypeConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<GameItemTypeModuleDtoToConfigConverter>();
        }
    }
}