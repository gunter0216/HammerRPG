using System.Collections.Generic;
using System.Linq;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateWalls
{
    public class CreateWallsDungeonGenerator : IDungeonGenerator
    {
        private const int m_WallSize = 1;
        
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.Dungeon.Data.RoomsData;
            var startRoom = roomsData.StartRoom;
            var rooms = roomsData.Rooms;
            ExpandRooms(null, startRoom.Connections[0].Room, 0);

            CreateWalls(rooms);
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateWalls(List<DungeonRoomData> rooms)
        {
            foreach (var room in rooms)
            {
                var tilesAmount = room.Width * 2 + room.Height * 2 - 4;
                var tiles = new List<TileData>(tilesAmount);
                room.Tiles = tiles;
                for (int i = room.Left; i < room.Right; ++i)
                {
                    tiles.Add(new TileData(TileConstants.Wall)
                    {
                        Position = new Vector2Int(i, room.Top - 1)
                    });
                    tiles.Add(new TileData(TileConstants.Wall)
                    {
                        Position = new Vector2Int(i, room.Bottom)
                    });
                }
                
                for (int i = room.Bottom + 1; i < room.Top - 1; ++i)
                {
                    tiles.Add(new TileData(TileConstants.Wall)
                    {
                        Position = new Vector2Int(room.Left, i)
                    });
                    tiles.Add(new TileData(TileConstants.Wall)
                    {
                        Position = new Vector2Int(room.Right - 1, i)
                    });
                }
            }
        }

        private void ExpandRooms(DungeonRoomData prevRoom, DungeonRoomData curRoom, int stupidCounter)
        {
            stupidCounter += 1;
            if (stupidCounter > 1000)
            {
                return;
            }
            
            var connections = curRoom.GetConnectionsExclude(prevRoom);

            foreach (var connection in connections)
            {
                if (connection.Side == RoomConnectSide.Top)
                {
                    curRoom.IncreaseHeight(m_WallSize);
                } 
                else if (connection.Side == RoomConnectSide.Bottom)
                {
                    curRoom.IncreaseHeight(m_WallSize);
                    curRoom.Move(Vector2Int.Bottom * m_WallSize);
                } 
                else if (connection.Side == RoomConnectSide.Right)
                {
                    curRoom.IncreaseWidth(m_WallSize);
                }
                else if (connection.Side == RoomConnectSide.Left)
                {
                    curRoom.IncreaseWidth(m_WallSize);
                    curRoom.Move(Vector2Int.Left * m_WallSize);
                }
            }

            foreach (var connection in connections)
            {
                ExpandRooms(curRoom, connection.Room, stupidCounter);
            }
        }

        public string GetName()
        {
            return "Create Walls";
        }
    }
}