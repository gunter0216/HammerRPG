using System;
using System.Linq;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors
{
    public class CreateDoorsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.Dungeon.Data.RoomsData;
            var startRoom = roomsData.StartRoom;
            var rooms = roomsData.Rooms;
            CreateDoor(null, startRoom, 0);

            generation.AddCash(new CreateDoorsGenerationCash());

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateDoor(DungeonRoomData prevRoom, DungeonRoomData curRoom, int stupidCounter)
        {
            stupidCounter += 1;
            if (stupidCounter > 1000)
            {
                return;
            }
            
            var connections = curRoom.GetConnectionsExclude(prevRoom);
            
            foreach (var connection in connections)
            {
                CreateDoor(curRoom, connection);
            }

            foreach (var connection in connections)
            {
                CreateDoor(curRoom, connection.Room, stupidCounter);
            }
        }

        private void CreateDoor(DungeonRoomData room, RoomConnection connection)
        {
            var otherRoom = connection.Room;
            var position = new Vector2Int();
            if (connection.Side == RoomConnectSide.Top ||
                connection.Side == RoomConnectSide.Bottom)
            {
                var left = Math.Max(room.Left, otherRoom.Left);
                var right = Math.Min(room.Right, otherRoom.Right);
                position.X = (left + right) / 2;
                if (connection.Side == RoomConnectSide.Top)
                {
                    position.Y = room.Top - 1;
                }
                else
                {
                    position.Y = room.Bottom;
                }
            }
            else if (connection.Side == RoomConnectSide.Right ||
                     connection.Side == RoomConnectSide.Left)
            {
                var top = Math.Min(room.Top, otherRoom.Top);
                var bottom = Math.Max(room.Bottom, otherRoom.Bottom);
                position.Y = (top + bottom) / 2;
                if (connection.Side == RoomConnectSide.Right)
                {
                    position.X = room.Right - 1;
                }
                else
                {
                    position.X = room.Left;
                }
            }
            
            // if (position.X == 0 || position.Y == 0)
            // {
            //     Debug.LogError($"Error \n {room} \n {connection.Room}");
            // }

            var worldPosition = position; 
            var localPosition = room.WorldToLocal(worldPosition);
            room.Matrix[localPosition.Y, localPosition.X].Id = TileConstants.Door;
            localPosition = otherRoom.WorldToLocal(worldPosition);
            otherRoom.Matrix[localPosition.Y, localPosition.X].Id = TileConstants.Door;

            // var tile = room.Tiles.FirstOrDefault(x => x.Position == position);
            // if (tile != null)
            // {
            //     tile.Id = TileConstants.Door;
            // }
            // else
            // {
            //     Debug.LogError($"arror");
            // }

            // todo add tile
        }

        public string GetName()
        {
            return "Create doors";
        }
    }
}