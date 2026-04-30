using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.ExpendRoom
{
    public class ExpendRoomDungeonGenerator : IDungeonGenerator
    {
        private const int m_WallSize = 1;
        
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = roomsData.StartGenerationRoom;
            var rooms = roomsData.Rooms;
            // ExpandRooms(null, startRoom, 0);

            foreach (var room in rooms)
            {
                ExpandRooms(room);
            }
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private void ExpandRooms(DungeonGenerationRoom room)
        {
            room.IncreaseHeight(m_WallSize * 2);
            room.IncreaseWidth(m_WallSize * 2);
            room.Move(Vector2Int.BottomLeft * m_WallSize * 2);
        }

        private void ExpandRooms(DungeonGenerationRoom prevGenerationRoom, DungeonGenerationRoom curGenerationRoom, int stupidCounter)
        {
            stupidCounter += 1;
            if (stupidCounter > 1000)
            {
                return;
            }
            
            var connections = curGenerationRoom.GetConnectionsExclude(prevGenerationRoom);

            foreach (var connection in connections)
            {
                if (connection.Side == RoomConnectSide.Top)
                {
                    curGenerationRoom.IncreaseHeight(m_WallSize);
                } 
                else if (connection.Side == RoomConnectSide.Bottom)
                {
                    curGenerationRoom.IncreaseHeight(m_WallSize);
                    curGenerationRoom.Move(Vector2Int.Bottom * m_WallSize);
                } 
                else if (connection.Side == RoomConnectSide.Right)
                {
                    curGenerationRoom.IncreaseWidth(m_WallSize);
                }
                else if (connection.Side == RoomConnectSide.Left)
                {
                    curGenerationRoom.IncreaseWidth(m_WallSize);
                    curGenerationRoom.Move(Vector2Int.Left * m_WallSize);
                }
            }

            foreach (var connection in connections)
            {
                ExpandRooms(curGenerationRoom, connection.GenerationRoom, stupidCounter);
            }
        }

        public string GetName()
        {
            return "Expend room";
        }
    }
}