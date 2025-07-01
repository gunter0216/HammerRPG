using System.Collections.Generic;
using App.Game.DungeonGenerator.Runtime.Rooms;
using DungeonGenerator.Matrices;
using UnityEngine;
using Random = System.Random;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators
{
    public class RoomCreateStrategy
    {
        private DungeonConfig m_Config;
        private System.Random m_Random;

        public RoomCreateStrategy()
        {
            m_Random = new Random();
        }

        public List<DungeonRoomData> CreateRooms(DungeonConfig dungeonConfig)
        {
            m_Config = dungeonConfig;
            
            var roomsAmount = m_Config.Rooms.CountRooms;
            var rooms = new List<DungeonRoomData>(roomsAmount);
            for (int i = 0; i < roomsAmount; ++i)
            {
                var position = GetRandomRoomPosition(m_Config.Rooms.RoomsRadius);
                var size = GetRandomRoomSize();

                var room = new DungeonRoomData(position, size);
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
            var width = m_Random.Next(m_Config.Rooms.MinWidthRoom, m_Config.Rooms.MaxWidthRoom + 1);
            var height = m_Random.Next(m_Config.Rooms.MinHeightRoom, m_Config.Rooms.MaxHeightRoom + 1);
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