using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Configs.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Contexts;
using App.Game.GameManagers.External.Config.Converter;
using App.Game.GameManagers.External.Config.Loader;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameManagers.External.Fabric;
using App.Game.GameManagers.External.Fabric.Room;
using App.Game.GameManagers.External.Fabric.Tile.View;
using App.Game.GameManagers.External.View;
using App.Game.GameTiles.External;
using App.Game.GameTiles.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.States.Runtime.Game;
using App.Game.Worlds.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Logger = App.Common.Logger.Runtime.Logger;
using Vector2 = App.Common.Algorithms.Runtime.Vector2;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.GameManagers.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 10)]
    public class GameManager : IInitSystem
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly TilesController m_TilesController;
        [Inject] private readonly IAssetManager m_AssetManager;
        [Inject] private readonly IWorldManager m_WorldManager;

        private DungeonGenerator m_Generator;
        private GenerationConfigService m_ConfigService;
        private DungeonGeneration m_Generation;
        private TileViewCreator m_TileViewCreator;

        private CreateRoomsResult m_CreateRoomsResult;

        public void Init()
        {
            return;
            
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