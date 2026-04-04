using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Chest.Runtime.Config
{
    public class ChestModuleConfig : IModuleConfig
    {
        private readonly string _closeIconKey;
        private readonly string _openIconKey;
        private readonly string _emptyIconKey;

        public string CloseIconKey => _closeIconKey;
        public string OpenIconKey => _openIconKey;
        public string EmptyIconKey => _emptyIconKey;

        public ChestModuleConfig(string closeIconKey, string openIconKey, string emptyIconKey)
        {
            _closeIconKey = closeIconKey;
            _openIconKey = openIconKey;
            _emptyIconKey = emptyIconKey;
        }
    }
}