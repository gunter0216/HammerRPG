using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using UnityEngine;

namespace App.Game.DungeonGenerator.External
{
    public class MonoDungeonGenerator : MonoBehaviour
    {
        [SerializeField] public DungeonConfig Config;
    }
}