using System.Collections.Generic;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Game.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Random = System.Random;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator
{
    public class RoomsCreator
    {
        private readonly RoomCreator m_RoomCreator;
        private readonly System.Random m_Random;
        
        private DungeonConfig m_Config;

        public RoomsCreator(RoomCreator roomCreator)
        {
            m_Random = new Random();
            m_RoomCreator = roomCreator;
        }

        public List<DungeonRoomData> CreateRooms(DungeonConfig dungeonConfig)
        {
            m_Config = dungeonConfig;
            
            var roomsAmount = m_Config.RoomsCreate.CountRooms;
            var rooms = new List<DungeonRoomData>(roomsAmount);
            for (int i = 0; i < roomsAmount; ++i)
            {
                var position = GetRandomRoomPosition(m_Config.RoomsCreate.RoomsRadius);
                var size = GetRandomRoomSize();
                var room = m_RoomCreator.Create(position, size);
                rooms.Add(room);
                // PlaceRoom(room);
            }

            return rooms;
        }
        
        // private void PlaceRoom(Room room)
        // {
        //     for (int i = 0; i < room.Height; ++i)
        //     {
        //         for (int j = 0; j < room.Width; ++j)
        //         {
        //             if (i == 0 || i == room.Height - 1 ||
        //                 j == 0 || j == room.Width - 1)
        //             {
        //                 m_Matrix.SetCell(room.Row + i, room.Col + j, Tile.RoomWall);
        //             }
        //             else
        //             {
        //                 m_Matrix.SetCell(room.Row + i, room.Col + j, Tile.Empty);
        //             }
        //         }
        //     }
        // }


        private Vector2Int GetRandomRoomSize()
        {
            var width = m_Random.Next(m_Config.RoomsCreate.MinWidthRoom, m_Config.RoomsCreate.MaxWidthRoom + 1);
            var height = m_Random.Next(m_Config.RoomsCreate.MinHeightRoom, m_Config.RoomsCreate.MaxHeightRoom + 1);
            return new Vector2Int(width, height);
        }

        private Vector2Int GetRandomRoomPosition(float radius = 1)
        {
            var randomPointInCircle = GetRandomPointInCircle(radius);
            return new Vector2Int((int)randomPointInCircle.x, (int)randomPointInCircle.y);
        }

        private Vector2 GetRandomPointInCircle(float radius)
        {
            return GetRandomPointInCircle() * radius;
        }

        private Vector2 GetRandomPointInCircle()
        {
            return UnityEngine.Random.insideUnitCircle;
        }
    }
}