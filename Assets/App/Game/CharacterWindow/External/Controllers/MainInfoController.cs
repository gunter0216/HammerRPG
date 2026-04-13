using App.Common.Logger.Runtime;
using App.Game.Modules.Experience.Runtime.Data;
using App.Game.Modules.Level.Runtime.Data;
using App.Game.Modules.Name.Runtime.Data;
using App.Game.Modules.Race.Runtime.Data;
using App.Game.Modules.Stats.Runtime.Data;
using App.Game.Player.External;

namespace App.Game.CharacterWindow.External.Controllers
{
    public class MainInfoController
    {
        private readonly View.CharacterWindow _window;
        private readonly PlayerController _playerController;
        
        private StatsModuleData _statsModuleData;
        private NameModuleData _nameModuleData;
        private LevelModuleData _levelModuleData;
        private ExperienceModuleData _experienceModuleData;
        private RaceModuleData _raceModuleData;

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
                HLogger.LogError($"StatsModuleData not found.");
                return;
            }
            
            if (!player.TryGetDataModule<NameModuleData>(out _nameModuleData))
            {
                HLogger.LogError($"NameModuleData not found.");
                return;
            }
            
            if (!player.TryGetDataModule<LevelModuleData>(out _levelModuleData))
            {
                HLogger.LogError($"LevelModuleData not found.");
                return;
            }
            
            if (!player.TryGetDataModule<ExperienceModuleData>(out _experienceModuleData))
            {
                HLogger.LogError($"ExperienceModuleData not found.");
                return;
            }
            
            if (!player.TryGetDataModule<RaceModuleData>(out _raceModuleData))
            {
                HLogger.LogError($"RaceModuleData not found.");
                return;
            }
        }

        public void UpdateInfo()
        {
            _window.StrengthStat.SetStatValue(_statsModuleData.Strength);
            _window.AgilityStat.SetStatValue(_statsModuleData.Agility);
            _window.IntelligenceStat.SetStatValue(_statsModuleData.Intelligence);
            
            _window.SetName(_nameModuleData.Name);
            
            _window.SetLevel(_levelModuleData.Level);
            
            _window.SetExperience(_experienceModuleData.Experience, 100);
            
            _window.SetRace(_raceModuleData.Race);
        }
    }
}