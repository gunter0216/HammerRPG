using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.Matrix;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateWalls
{
    public class CreateWallsDungeonGenerator : IDungeonGenerator
    {
        private DungeonGenerationRoom _startRoom;
        private DungeonGenerationRoom _nextRoom;
        private const int m_WallSize = 1;
        
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var rooms = roomsData.Rooms;

            _startRoom = roomsData.StartGenerationRoom;
            _nextRoom = _startRoom.Connections.First().GenerationRoom;
            
            CreateWalls(rooms);
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateWalls(List<DungeonGenerationRoom> rooms)
        {
            foreach (var room in rooms)
            {
                CreateWalls(room);
            }
            
            foreach (var room in rooms)
            {
                CreateCorridor(room);
            }
        }

        private void CreateWalls(DungeonGenerationRoom room)
        {
            for (int i = 0; i < room.Width; ++i)
            {
                room.SetTile(i, 0, DungeonTile.Wall);
                room.SetTile(i, room.Height - 1, DungeonTile.Wall);
            }
                 
            for (int i = 1; i < room.Height - 1; ++i)
            {
                room.SetTile(0, i, DungeonTile.Wall);
                room.SetTile(room.Width - 1, i, DungeonTile.Wall);
            }
        }

        private void CreateCorridor(DungeonGenerationRoom room)
        {
            var corridor = room.Corridor;
            if (corridor != null)
            {
                var area = corridor.Area;
                var size = area.Size;
                var position = area.Position;
                var connectRoom = room.Connections.First(x => corridor.Side == x.Side).GenerationRoom;
                Vector2Int position1;
                Vector2Int position2;
                if (corridor.IsHorizontal)
                {
                    var width = size.X;
                    for (int i = 0; i < width - 2; ++i)
                    {
                        room.SetTile(position.X + i + 1, position.Y + 0, DungeonTile.Wall);
                        room.SetTile(position.X + i + 1, position.Y + size.Y - 1, DungeonTile.Wall);
                    }

                    position1 = new Vector2Int(position.X, position.Y + size.Y / 2);
                    position2 = new Vector2Int(position.X + size.X - 1, position.Y + size.Y / 2);
                }
                else
                {
                    var height = size.Y;
                    for (int i = 0; i < height - 2; ++i)
                    {
                        room.SetTile(position.X + 0, position.Y + i + 1, DungeonTile.Wall);
                        room.SetTile(position.X + size.X - 1, position.Y + i + 1, DungeonTile.Wall);
                    }
                    
                    position1 = new Vector2Int(position.X + size.X / 2, position.Y);
                    position2 = new Vector2Int(position.X + size.X / 2, position.Y + size.Y - 1);
                }
                
                room.RemoveTile(position1);
                room.RemoveTile(position2);
                var otherRoomPosition1 = connectRoom.WorldToLocal(room.LocalToWorld(position1));
                var otherRoomPosition2 = connectRoom.WorldToLocal(room.LocalToWorld(position2));
                connectRoom.RemoveTile(otherRoomPosition1);
                connectRoom.RemoveTile(otherRoomPosition2);
                Vector2Int doorPosition = new Vector2Int();
                if (corridor.Side == RoomConnectSide.Right)
                {
                    doorPosition = position2;
                } 
                else if (corridor.Side == RoomConnectSide.Left)
                {
                    doorPosition = position1;
                }
                else if (corridor.Side == RoomConnectSide.Top)
                {
                    doorPosition = position2;
                }
                else if (corridor.Side == RoomConnectSide.Bottom)
                {
                    doorPosition = position1;
                }
                
                room.SetTile(doorPosition, DungeonTile.Door);
            }
        }

        public string GetName()
        {
            return "Create Walls";
        }
    }
}