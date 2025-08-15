using System;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors
{
    public class CreateDoorsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = roomsData.StartGenerationRoom;
            var rooms = roomsData.Rooms;
            CreateDoor(null, startRoom, 0);

            generation.AddCash(new CreateDoorsGenerationCash());

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateDoor(DungeonGenerationRoom prevGenerationRoom, DungeonGenerationRoom curGenerationRoom, int stupidCounter)
        {
            stupidCounter += 1;
            if (stupidCounter > 1000)
            {
                return;
            }
            
            var connections = curGenerationRoom.GetConnectionsExclude(prevGenerationRoom);
            
            foreach (var connection in connections)
            {
                CreateDoor(curGenerationRoom, connection);
            }

            foreach (var connection in connections)
            {
                CreateDoor(curGenerationRoom, connection.GenerationRoom, stupidCounter);
            }
        }

        private void CreateDoor(DungeonGenerationRoom generationRoom, RoomConnection connection)
        {
            var otherRoom = connection.GenerationRoom;
            var position = new Vector2Int();
            if (connection.Side == RoomConnectSide.Top ||
                connection.Side == RoomConnectSide.Bottom)
            {
                var left = Math.Max(generationRoom.Left, otherRoom.Left);
                var right = Math.Min(generationRoom.Right, otherRoom.Right);
                position.X = (left + right) / 2;
                if (connection.Side == RoomConnectSide.Top)
                {
                    position.Y = generationRoom.Top - 1;
                }
                else
                {
                    position.Y = generationRoom.Bottom;
                }
            }
            else if (connection.Side == RoomConnectSide.Right ||
                     connection.Side == RoomConnectSide.Left)
            {
                var top = Math.Min(generationRoom.Top, otherRoom.Top);
                var bottom = Math.Max(generationRoom.Bottom, otherRoom.Bottom);
                position.Y = (top + bottom) / 2;
                if (connection.Side == RoomConnectSide.Right)
                {
                    position.X = generationRoom.Right - 1;
                }
                else
                {
                    position.X = generationRoom.Left;
                }
            }

            var worldPosition = position; 
            var localPosition = generationRoom.WorldToLocal(worldPosition);
            generationRoom.Matrix[localPosition.Y, localPosition.X].Id = TileConstants.Door;
            localPosition = otherRoom.WorldToLocal(worldPosition);
            otherRoom.Matrix[localPosition.Y, localPosition.X].Id = TileConstants.Door;
        }

        public string GetName()
        {
            return "Create doors";
        }
    }
}