using App.Common.AssetSystem.Runtime;
using App.Common.Input.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCore.Runtime;
using App.Game.Modules.Experience.Runtime.Config;
using App.Game.Modules.Experience.Runtime.Data;
using App.Game.Modules.Level.Runtime.Config;
using App.Game.Modules.Level.Runtime.Data;
using App.Game.Modules.Move.Runtime;
using App.Game.Modules.Name.Runtime.Config;
using App.Game.Modules.Name.Runtime.Data;
using App.Game.Modules.Race.Runtime.Config;
using App.Game.Modules.Race.Runtime.Data;
using App.Game.Modules.Stats.Runtime.Config;
using App.Game.Modules.Stats.Runtime.Data;
using App.Game.Player.External.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Player.External
{
    public class PlayerController : IInitSystem, IUpdateSystem
    {
        private readonly IDungeonController _dungeonController;
        private readonly IAssetManager _assetManager;
        private readonly MoveModuleSystem _moveModuleSystem;
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IInputService _inputService;
        
        private EntityView _view;
        private IModuleItem _player;
        private MoveModule _moveModule;
        private InputAction _moveInput;
        private PlayerMoveController _playerMoveController;

        public EntityView PlayerView => _view;

        public IModuleItem Player => _player;

        public PlayerController(
            IDungeonController dungeonController, 
            IAssetManager assetManager,
            MoveModuleSystem moveModuleSystem, 
            IModuleItemsManager moduleItemsManager, 
            IInputService inputService)
        {
            _dungeonController = dungeonController;
            _assetManager = assetManager;
            _moveModuleSystem = moveModuleSystem;
            _moduleItemsManager = moduleItemsManager;
            _inputService = inputService;
        }

        public void Init()
        {
            var moduleItem = _moduleItemsManager.Create("player");
            if (!moduleItem.HasValue)
            {
                HLogger.LogError("Cant create player.");
                return;
            }

            _player = moduleItem.Value;
            CreateView();
            PlacePlayerOnStartRoom();
            InitMove();
            InitStats();
            InitName();
            InitLevel();
            InitExperience();
            InitRace();
        }

        private void CreateView()
        {
            var viewCreator = new PlayerViewCreator(_assetManager);
            var viewResult = viewCreator.Create();
            if (!viewResult.HasValue)
            {
                HLogger.LogError("Cant create view.");
                return;
            }

            _view = viewResult.Value;
        }

        private void PlacePlayerOnStartRoom()
        {
            var position = _dungeonController.GetSpawnPoint();
            if (!position.HasValue)
            {
                return;
            }
            
            PlayerView.transform.position = new Vector3(position.Value.X, 1, position.Value.Y);
        }

        private void InitMove()
        {
            _playerMoveController = new PlayerMoveController(
                _moveModuleSystem, 
                _inputService, 
                Player,
                PlayerView);
            _playerMoveController.Init();
        }

        private void InitStats()
        {
            if (!_player.TryGetDataModule<StatsModuleData>(out var statsModuleData))
            {
                if (_player.TryGetConfigModule<StatsModuleConfig>(out var config))
                {
                    statsModuleData = new StatsModuleData()
                    {
                        Agility = config.Agility,
                        Strength = config.Strength,
                        Intelligence = config.Intelligence
                    };
                    
                    _player.AddDataModule(statsModuleData);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitName()
        {
            if (!_player.TryGetDataModule<NameModuleData>(out var data))
            {
                if (_player.TryGetConfigModule<NameModuleConfig>(out var config))
                {
                    data = new NameModuleData()
                    {
                        Name = config.Name,
                    };
                    
                    _player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitExperience()
        {
            if (!_player.TryGetDataModule<ExperienceModuleData>(out var data))
            {
                if (_player.TryGetConfigModule<ExperienceModuleConfig>(out var config))
                {
                    data = new ExperienceModuleData();
                    
                    _player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitLevel()
        {
            if (!_player.TryGetDataModule<LevelModuleData>(out var data))
            {
                if (_player.TryGetConfigModule<LevelModuleConfig>(out var config))
                {
                    data = new LevelModuleData()
                    {
                        Level = config.StartLevel,
                    };
                    
                    _player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitRace()
        {
            if (!_player.TryGetDataModule<RaceModuleData>(out var data))
            {
                if (_player.TryGetConfigModule<RaceModuleConfig>(out var config))
                {
                    data = new RaceModuleData()
                    {
                        Race = config.Race,
                    };
                    
                    _player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        public void OnUpdate()
        {
            _playerMoveController.OnUpdate();
        }
    }
}