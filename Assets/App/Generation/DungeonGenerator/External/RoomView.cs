using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] 
        private RoomGenerationConfig _generationConfig;

        public RoomGenerationConfig GenerationConfig => _generationConfig;
    }
}