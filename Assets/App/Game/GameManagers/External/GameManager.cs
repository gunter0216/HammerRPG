using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using App.Game.GameManagers.External.Config.Converter;
using App.Game.GameManagers.External.Config.Loader;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameManagers.External.Services;
using App.Game.GameManagers.External.View;
using App.Game.GameTiles.External;
using App.Game.GameTiles.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.States.Game;
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

        private List<GameRoom> m_Rooms;
        private GameRoom m_EndRoom;
        private GameRoom m_StartRoom;

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

            CreateRooms();
            
            if (!CreateTiles())
            {
                HLogger.LogError("Cant create tiles");
                return;
            }

            PlacePlayerOnStartRoom();
        }

        private void CreateRooms()
        {
            var generationRooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = generationRooms.StartGenerationRoom;
            var endRoom = generationRooms.EndGenerationRoom;
            var rooms = generationRooms.Rooms;

            m_Rooms = new List<GameRoom>(rooms.Count);
            foreach (var generationRoom in rooms)
            {
                var content = new GameObject();
                var view = new RoomView(content.transform);
                var room = new GameRoom(view, generationRoom);
                m_Rooms.Add(room);

                if (startRoom == generationRoom)
                {
                    m_StartRoom = room;
                } 
                else if (endRoom == generationRoom)
                {
                    m_EndRoom = room;
                }
            }
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

        private bool CreateTiles()
        {
            m_TileViewCreator = new TileViewCreator(m_AssetManager);
            if (!m_TileViewCreator.Initialize())
            {
                return false;
            }
            
            foreach (var room in m_Rooms)
            {
                CreateRoom(room);
            }

            return true;
        }

        private void CreateRoom(GameRoom room)
        {
            var matrix = room.GenerationRoom.Matrix;
            for (int i = 0; i < matrix.Height; ++i)
            {
                for (int j = 0; j < matrix.Width; ++j)
                {
                    var tile = matrix[i, j];
                    var gameTile = CreateTile(room, tile, new Vector2Int(j, i));
                    // todo
                }
            }

            CreateFloor(room);
        }

        private void CreateFloor(GameRoom room)
        {
            var sprite = m_TilesController.GetTileSprite("floor");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }

            var floor = new GameObject();
            var position = room.GenerationRoom.GetCenter();
            floor.transform.position = new Vector3(position.X - 0.5f, position.Y - 0.5f, 1);
            floor.transform.parent = room.View.Content;
            var spriteRenderer = floor.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.Value;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new UnityEngine.Vector2(
                room.GenerationRoom.Width, 
                room.GenerationRoom.Height);
        }

        private Optional<GameTile> CreateTile(GameRoom room, GeneraitonTile generationTile, Vector2Int localPosition)
        {
            var worldPosition = room.GenerationRoom.LocalToWorld(localPosition);
            
            if (generationTile.Id == TileConstants.Empty)
            {
                return Optional<GameTile>.Fail();
            }

            var isDoor = generationTile.Id == TileConstants.Door;
            var hasLockedDoor = room.GenerationRoom.RequiredKey != null;

            var tile = m_TilesController.CreateTileByGenerationID(generationTile.Id, worldPosition);
            if (!tile.HasValue)
            {
                HLogger.LogError("Cant create tile");
                return Optional<GameTile>.Fail();
            }

            var sprite = !isDoor || hasLockedDoor ? m_TilesController.GetTileSprite(tile.Value)
                    : m_TilesController.GetTileSprite("opened_door");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return Optional<GameTile>.Fail();
            }
            
            var view = CreateTileView();
            if (!view.HasValue)
            {
                HLogger.LogError("Cant create tile view");
                return Optional<GameTile>.Fail();
            }

            view.Value.SetParent(room.View.Content);
            view.Value.SetSprite(sprite.Value);
            view.Value.SetPosition(new Vector3(worldPosition.X, worldPosition.Y));
            if (isDoor && !hasLockedDoor)
            {
                view.Value.DisableCollider();
            }

            var gameTile = new GameTile(tile.Value, view.Value);
            return Optional<GameTile>.Success(gameTile);
        }

        private Optional<ITileView> CreateTileView()
        {
            return m_TileViewCreator.Create();
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
    }
}