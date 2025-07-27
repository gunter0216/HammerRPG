using App.Common.Utility.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation
{
    public interface IDungeonGenerator
    {
        Optional<DungeonGeneration> Process(DungeonGeneration generation);
        string GetName();
    }
}