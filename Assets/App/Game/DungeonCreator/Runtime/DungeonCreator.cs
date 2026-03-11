using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCreator.Runtime.Rooms;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameTiles.Runtime;
using Logger = App.Common.Logger.Runtime.Logger;

namespace App.Generation.DungeonCreator.Runtime
{
    public class DungeonCreator : IDungeonCreator, IInitSystem
    {
        private readonly IConfigLoader _configLoader;
        private readonly ITilesController _tilesController;

        private GenerationConfigController _configController;

        public DungeonCreator(
            IConfigLoader configLoader,
            ITilesController tilesController)
        {
            _configLoader = configLoader;
            _tilesController = tilesController;
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

            var generator = new DungeonGenerator.Runtime.DungeonGenerators.DungeonGenerator(new Logger());
            var dungeonGeneration = generator.Generate(generationConfig.Value);
            if (!dungeonGeneration.HasValue)
            {
                HLogger.LogError("Cant generate");
                return Optional<Dungeon>.Fail();
            }

            var data = new DungeonData();
            var dungeon = new Dungeon(data, generationConfig.Value);

            var roomsCreator = new RoomsCreator(_tilesController);
            roomsCreator.CreateRooms(dungeonGeneration.Value, dungeon);

            return Optional<Dungeon>.Success(dungeon);
        }
    }
}