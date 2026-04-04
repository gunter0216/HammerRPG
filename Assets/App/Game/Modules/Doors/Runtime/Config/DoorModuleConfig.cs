using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Door.Runtime.Config
{
    public class DoorModuleConfig : IModuleConfig
    {
        private readonly string _closeIconKey;
        private readonly string _openIconKey;

        public string CloseIconKey => _closeIconKey;
        public string OpenIconKey => _openIconKey;

        public DoorModuleConfig(string closeIconKey, string openIconKey)
        {
            _closeIconKey = closeIconKey;
            _openIconKey = openIconKey;
        }
    }
}