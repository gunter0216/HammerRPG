using App.Common.GameItem.External.Dto;
using App.Common.GameItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.External.Config.Interfaces
{
    public interface IModuleDtoToConfigConverter
    {
        Optional<IModuleConfig> Convert(GameItemModulesDto modules);
    }
}