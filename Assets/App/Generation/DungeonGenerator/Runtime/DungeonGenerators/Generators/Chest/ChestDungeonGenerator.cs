using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Chest
{
    public class ChestDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var rooms = roomsData.Rooms;

            CreateChests(rooms);
            
            // todo test
            var startRoom = roomsData.StartGenerationRoom;
            startRoom.AddDoorKey(new DungeonKeyData(-1));
            startRoom.Chests.Add(new DungeonGenerationChest(startRoom.LocalCenter.ToInt(), startRoom.ContainsDoorKeys[0]));

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateChests(List<DungeonGenerationRoom> rooms)
        {
            foreach (var room in rooms)
            {
                CreateChest(room);
            }
        }

        private void CreateChest(DungeonGenerationRoom room)
        {
            if (room.ContainsDoorKeys != null && room.ContainsDoorKeys.Count > 0)
            {
                room.Chests.Add(new DungeonGenerationChest(room.LocalCenter.ToInt(), room.ContainsDoorKeys[0]));
            }
        }

        public string GetName()
        {
            return "Create Chest";
        }
    }
}