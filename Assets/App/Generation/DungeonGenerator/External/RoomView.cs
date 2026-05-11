using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] 
        private RoomConfigAsset _config;

        public RoomConfigAsset Config => _config;
    }
}