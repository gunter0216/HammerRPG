using App.Common.AssetSystem.Runtime;
using App.Common.Input.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCore.Runtime;
using App.Game.Modules.Move.Runtime;
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
            
            PlayerView.transform.position = new Vector3(position.Value.X, position.Value.Y);
        }

        private void InitMove()
        {
            _playerMoveController = new PlayerMoveController(
                _moveModuleSystem, 
                _inputService, 
                _player,
                PlayerView);
            _playerMoveController.Init();
        }

        public void OnUpdate()
        {
            _playerMoveController.OnUpdate();
        }
    }
}