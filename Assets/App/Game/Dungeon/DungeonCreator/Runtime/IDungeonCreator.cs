using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Dungeon.DungeonCreator.Runtime
{
    public interface IDungeonCreator
    {
        Optional<Dungeon> Create(string generationKey = "Default");
    }
}