namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    public enum RoomType
    {
        // Explicit values preserve existing serialized Start and Transit rooms.
        Start = 2,
        Transit = 1,
        Content = 3,
    }
}
