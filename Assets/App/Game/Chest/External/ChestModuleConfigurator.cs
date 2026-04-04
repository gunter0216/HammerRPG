using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Generation.DungeonCreator.Runtime.Chest;

namespace App.Game.Chest.External
{
    [Configurator(DIContext.CoreContext)]
    public class ChestModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ChestModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalChestModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ChestModuleDtoToConfigConverter>();
            
            RegisterData<ChestContainerData>();
        }
    }
}