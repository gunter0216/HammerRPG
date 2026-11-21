using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    public class RoomGenerationConfig
    {
        public string ID { get; }
        public RoomType RoomType { get; }
        public Vector2Int Size { get; }
        public Vector2Int InputDoor { get; }
        public Vector2Int PrefabOrigin { get; }
        public RoomConfigVariant Variant { get; }

        public RoomGenerationConfig(string id, RoomType roomType, Vector2Int size, Vector2Int inputDoor,
            Vector2Int prefabOrigin, RoomConfigVariant variant)
        {
            ID = id;
            RoomType = roomType;
            Size = size;
            InputDoor = inputDoor;
            PrefabOrigin = prefabOrigin;
            Variant = variant;
        }
    }
}
