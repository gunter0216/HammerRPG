using UnityEngine.AddressableAssets;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    public class RoomConfigVariant
    {
        public string ID { get; }
        public AssetReferenceGameObject AssetReference { get; }
        public Vector2Int[] OutputDoors { get; }

        public RoomConfigVariant(string id, AssetReferenceGameObject assetReference, Vector2Int[] outputDoors)
        {
            ID = id;
            AssetReference = assetReference;
            OutputDoors = outputDoors;
        }
    }
}
