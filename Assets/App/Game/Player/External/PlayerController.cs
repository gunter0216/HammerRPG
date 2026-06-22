using App.Common.AssetSystem.Runtime;
using App.Common.Input.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Game.Dungeon.DungeonCore.Runtime;
using App.Game.Modules.Move.Runtime;
using App.Game.Player.External.Attack;
using App.Game.Player.External.Context;
using App.Game.Player.External.Items;
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
        private readonly IWindowManager _windowManager;
        
        private EntityView _view;
        private IModuleItem _player;
        private MoveModule _moveModule;
        private InputAction _moveInput;
        private PlayerMoveController _playerMoveController;
        private PlayerAttackController _playerAttackController;
        private PlayerContext _context;
        private LeftClickController _leftClickController;
        private HandItemsController _handItemsController;

        public EntityView PlayerView => _view;

        public IModuleItem Player => _player;

        public PlayerController(
            IDungeonController dungeonController, 
            IAssetManager assetManager,
            MoveModuleSystem moveModuleSystem, 
            IModuleItemsManager moduleItemsManager, 
            IInputService inputService, IWindowManager windowManager)
        {
            _dungeonController = dungeonController;
            _assetManager = assetManager;
            _moveModuleSystem = moveModuleSystem;
            _moduleItemsManager = moduleItemsManager;
            _inputService = inputService;
            _windowManager = windowManager;
        }

        public void Init()
        {
            var playerCreator = new PlayerCreator(_moduleItemsManager);
            _player = playerCreator.Create();
            
            CreateView();
            PlacePlayerOnStartRoom();
            InitContext();
            InitMove();
            InitAttack();
            InitLeftClickController();
            InitHandItems();
        }

        private void InitHandItems()
        {
            _handItemsController = new HandItemsController(_context, _assetManager);
            _handItemsController.Initialize();

            var itemResult = _moduleItemsManager.Create("IronSword");
            var item = itemResult.Value;
            _handItemsController.Equip(new EquipHandItemInfo(EHand.Right, item));
        }

        private void InitLeftClickController()
        {
            _leftClickController = new LeftClickController(
                _inputService, 
                _windowManager,
                _context,
                _playerAttackController);
            _leftClickController.Initialize();
        }

        private void InitContext()
        {
            _context = new PlayerContext
            {
                ModuleItem = _player,
                View = _view,
                RigProvider = _view.GetComponent<RigProviderView>()
            };
        }

        private void CreateView()
        {
            var viewCreator = new PlayerViewCreator(_assetManager);
            var viewResult = viewCreator.Create(_player);
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
                _context);
            _playerMoveController.Init();
        }

        private void InitAttack()
        {
            _playerAttackController = new PlayerAttackController(_context);
            _playerAttackController.Initialize();
        }

        public void OnUpdate()
        {
            _playerMoveController.OnUpdate();
            _leftClickController.OnUpdate();
        }
    }
}