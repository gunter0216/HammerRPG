using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Controller;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;
using App.Game.GameTiles.Runtime;
using App.Game.Modules.Chest.Runtime;
using App.Game.Modules.ContainerModule.Runtime;
using Logger = App.Common.Logger.Runtime.Logger;

namespace App.Game.Dungeon.DungeonCreator.Runtime
{
    public class DungeonCreator : IDungeonCreator, IInitSystem
    {
        private readonly ChestModuleSystem _chestModuleSystem;
        private readonly ContainerModuleSystem _containerModuleSystem;
        private readonly IConfigLoader _configLoader;
        private readonly ITilesController _tilesController;
        private readonly IModuleItemsManager _moduleItemsManager;

        private GenerationConfigController _configController;

        public DungeonCreator(
            IConfigLoader configLoader,
            ITilesController tilesController, 
            IModuleItemsManager moduleItemsManager, 
            ChestModuleSystem chestModuleSystem, 
            ContainerModuleSystem containerModuleSystem)
        {
            _configLoader = configLoader;
            _tilesController = tilesController;
            _moduleItemsManager = moduleItemsManager;
            _chestModuleSystem = chestModuleSystem;
            _containerModuleSystem = containerModuleSystem;
        }

        public void Init()
        {
            if (!InitConfig())
            {
                HLogger.LogError("Cant inti config service.");
                return;
            }
        }

        private bool InitConfig()
        {
            _configController = new GenerationConfigController(_configLoader);
            _configController.Initialize();

            return true;
        }

        public Optional<Dungeon> Create(string generationKey)
        {
            var generationConfig = _configController.GetGeneration();
            if (!generationConfig.HasValue)
            {
                HLogger.LogError("Config not found");
                return Optional<Dungeon>.Fail();
            }

            var generator = new Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonGenerator(new Logger());
            var dungeonGeneration = generator.Generate(generationConfig.Value);
            if (!dungeonGeneration.HasValue)
            {
                HLogger.LogError("Cant generate"); 
                return Optional<Dungeon>.Fail();
            }

            var data = new DungeonData();
            var dungeon = new Dungeon(data, generationConfig.Value);

            var roomsCreator = new RoomsCreator(
                _tilesController, 
                _moduleItemsManager, 
                _chestModuleSystem, 
                _containerModuleSystem);
            roomsCreator.CreateRooms(dungeonGeneration.Value, dungeon);

            return Optional<Dungeon>.Success(dungeon);
        }
    }
}