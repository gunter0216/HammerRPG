using App.Generation.DungeonGenerator.External.Dto;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External
{
    public class MonoDungeonGenerator : MonoBehaviour
    {
        [SerializeField] public DungeonGenerationConfigDto Config;
        [SerializeField] public bool ShowLabel;
    }
}