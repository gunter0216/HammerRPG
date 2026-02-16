using App.Common.Utilities.Utility.Runtime;

namespace App.Generation.DungeonCreator.Runtime
{
    public interface IDungeonCreator
    {
        Optional<Dungeon> Create(string generationKey = "Default");
    }
}