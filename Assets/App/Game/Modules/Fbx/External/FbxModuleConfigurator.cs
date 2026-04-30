using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Converter;

namespace App.Game.Modules.ModuleItemType.External
{
    [Configurator(DIContext.GlobalContext)]
    public class FbxModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<FbxModuleDtoToConfigConverter>();
        }
    }
}