using App.Common.Utility.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndRooms
{
    public class StartEndRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            
        }

        public string GetName()
        {
            return "Select Start and End rooms";
        }
    }
}