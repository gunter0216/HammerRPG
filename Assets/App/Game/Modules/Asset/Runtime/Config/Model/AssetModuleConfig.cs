using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model
{
    public class AssetModuleConfig : IModuleConfig
    {
        private readonly string _assetKey;

        public string AssetKey => _assetKey;

        public AssetModuleConfig(string assetKey)
        {
            _assetKey = assetKey;
        }
    }
}