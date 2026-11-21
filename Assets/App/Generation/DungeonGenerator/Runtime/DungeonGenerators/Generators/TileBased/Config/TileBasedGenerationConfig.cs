
namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    public class TileBasedGenerationConfig : IGenerationConfig
    {
        public RoomGenerationConfig[] Rooms { get; }
        public int MaxDepth { get; }
        public int MaxIterations { get; }

        public TileBasedGenerationConfig(RoomGenerationConfig[] rooms, int maxDepth, int maxIterations)
        {
            Rooms = rooms;
            MaxDepth = maxDepth;
            MaxIterations = maxIterations;
        }
    }
}
