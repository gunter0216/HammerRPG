using App.Common.Logger.Runtime;
using App.Game.Modules.Stats.Runtime.Data;
using App.Game.Player.External;

namespace App.Game.CharacterWindow.External.Controllers
{
    public class MainInfoController
    {
        private readonly View.CharacterWindow _window;
        private readonly PlayerController _playerController;
        
        private StatsModuleData _statsModuleData;

        public MainInfoController(View.CharacterWindow window, PlayerController playerController)
        {
            _window = window;
            _playerController = playerController;
        }

        public void Init()
        {
            var player = _playerController.Player;
            if (!player.TryGetDataModule<StatsModuleData>(out _statsModuleData))
            {
                HLogger.LogError($"statsModuleData not found.");
                return;
            }
        }

        public void UpdateInfo()
        {
            _window.StrengthStat.SetStatValue(_statsModuleData.Strength);
            _window.AgilityStat.SetStatValue(_statsModuleData.Agility);
            _window.IntelligenceStat.SetStatValue(_statsModuleData.Intelligence);
        }
    }
}