using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Config.Converter;
using App.Game.GameManagers.External.Config.Loader;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameManagers.External.Fabric;
using App.Game.GameManagers.External.Fabric.Room;
using App.Game.GameManagers.External.Fabric.Tile.View;
using App.Game.GameTiles.External;
using App.Game.Player.Runtime.Components;
using App.Game.Worlds.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using UnityEngine;
using Logger = App.Common.Logger.Runtime.Logger;

namespace App.Game.GameManagers.External
{
    public class GameManager : IInitSystem
    {
        private readonly IConfigLoader m_ConfigLoader;
        private readonly TilesController m_TilesController;
        private readonly IAssetManager m_AssetManager;
        private readonly IWorldManager m_WorldManager;

        private DungeonGenerator m_Generator;
        private GenerationConfigService m_ConfigService;
        private DungeonGeneration m_Generation;
        private TileViewCreator m_TileViewCreator;

        private CreateRoomsResult m_CreateRoomsResult;

        public GameManager(
            IConfigLoader configLoader,
            TilesController tilesController,
            IAssetManager assetManager,
            IWorldManager worldManager)
        {
            m_ConfigLoader = configLoader;
            m_TilesController = tilesController;
            m_AssetManager = assetManager;
            m_WorldManager = worldManager;
        }

        public void Init()
        {
            if (!InitConfig())
            {
                HLogger.LogError("Cant inti config service.");
                return;
            }
            
            if (!CreateGeneration())
            {
                HLogger.LogError("Cant generate dungeon");
                return;
            }

            var roomsCreator = new RoomsCreator(m_AssetManager, m_TilesController);
            var result = roomsCreator.CreateRooms(m_Generation);
            m_CreateRoomsResult = result.Value;

            PlacePlayerOnStartRoom();
        }

        private bool InitConfig()
        {
            var loader = new GenerationConfigLoader(m_ConfigLoader);
            var converter = new GenerationDtoToConfigConverter();
            var dto = loader.Load();
            if (!dto.HasValue)
            {
                return false;
            }

            var config = converter.Convert(dto.Value);
            if (!config.HasValue)
            {
                return false;
            }
            
            m_ConfigService = new GenerationConfigService(config.Value);
            
            return true;
        }

        private bool CreateGeneration()
        {
            var generationConfig = m_ConfigService.GetGeneration();
            m_Generator = new DungeonGenerator(new Logger());
            var dungeonGeneration = m_Generator.Generate(generationConfig);
            if (!dungeonGeneration.HasValue)
            {
                return false;
            }

            m_Generation = dungeonGeneration.Value;
            return true;
        }

        private void PlacePlayerOnStartRoom()
        {
            var generationRooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = generationRooms.StartGenerationRoom;
            var position = startRoom.GetCenter();
            
            var world = m_WorldManager.GetWorld();
            var entityPool = world.GetPool<EntityComponent>();

            foreach (var i in world.Filter<PlayerComponent>().End())
            {
                var entity = entityPool.Get(i);
                entity.View.transform.position = new Vector3(position.X, position.Y);
            }
        }
    }
}