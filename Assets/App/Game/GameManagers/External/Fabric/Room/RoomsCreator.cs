using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameManagers.External.Fabric.Room.Floor;
using App.Game.GameManagers.External.Fabric.Room.View;
using App.Game.GameManagers.External.Fabric.Tile;
using App.Game.GameManagers.External.Fabric.Tile.View;
using App.Game.GameManagers.External.Room;
using App.Game.GameTiles.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.GameManagers.External.Fabric.Room
{
    public class RoomsCreator
    {
        private readonly TilesController m_TilesController;
        private readonly IAssetManager m_AssetManager;

        private GenerationConfigService m_ConfigService;
        private DungeonGeneration m_Generation;
        private TileViewCreator m_TileViewCreator;

        private RoomCreator m_RoomCreator;
        private GameTileCreator m_GameTileCreator;
        
        private List<GameRoom> m_Rooms;
        private GameRoom m_EndRoom;
        private GameRoom m_StartRoom;

        private RoomFloorCreator m_RoomFloorCreator;
        
        private Transform m_RoomsParent;

        public RoomsCreator(IAssetManager assetManager, TilesController tilesController)
        {
            m_AssetManager = assetManager;
            m_TilesController = tilesController;

            var roomViewCreator = new RoomViewCreator();
            m_RoomCreator = new RoomCreator(roomViewCreator);
            
            m_TileViewCreator = new TileViewCreator(m_AssetManager);
            m_TileViewCreator.Initialize();

            m_GameTileCreator = new GameTileCreator(m_TilesController, m_TileViewCreator);

            m_RoomFloorCreator = new RoomFloorCreator(m_TilesController);
        }

        public Optional<CreateRoomsResult> CreateRooms(DungeonGeneration generation)
        {
            m_Generation = generation;

            m_RoomsParent = new GameObject("Rooms").transform;
            
            CreateRooms();
            
            var result = new CreateRoomsResult(m_Rooms, m_EndRoom, m_StartRoom);
            
            return Optional<CreateRoomsResult>.Success(result);
        }
        
        private bool CreateRooms()
        {
            var generationRooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = generationRooms.StartGenerationRoom;
            var endRoom = generationRooms.EndGenerationRoom;
            var rooms = generationRooms.Rooms;

            m_Rooms = new List<GameRoom>(rooms.Count);
            foreach (var generationRoom in rooms)
            {
                var room = CreateRoom(generationRoom);
                if (!room.HasValue)
                {
                    return false;
                }
                
                room.Value.SetParent(m_RoomsParent);
                
                m_Rooms.Add(room.Value);

                if (startRoom == generationRoom)
                {
                    m_StartRoom = room.Value;
                } 
                else if (endRoom == generationRoom)
                {
                    m_EndRoom = room.Value;
                }
            }

            return false;
        }

        private Optional<GameRoom> CreateRoom(DungeonGenerationRoom generationRoom)
        {
            var room = m_RoomCreator.Create(generationRoom);
            if (!room.HasValue)
            {
                return Optional<GameRoom>.Fail();
            }

            CreateTiles(room.Value);
            CreateFloor(room.Value);
            
            return Optional<GameRoom>.Success(room.Value);
        }

        private void CreateFloor(GameRoom room)
        {
            m_RoomFloorCreator.CreateFloor(room);
        }

        private void CreateTiles(GameRoom room)
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
        }

        private Optional<GameTile> CreateTile(GameRoom room, GeneraitonTile generationTile, Vector2Int localPosition)
        {
            return m_GameTileCreator.CreateTile(room, generationTile, localPosition);
        }
    }
}