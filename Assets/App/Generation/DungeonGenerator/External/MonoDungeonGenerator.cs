using App.Generation.DungeonGenerator.Runtime.Configs.Settings;
using UnityEngine;
using App.Generation.DungeonGenerator.Runtime.Configs;

namespace App.Generation.DungeonGenerator.External
{
    public class MonoDungeonGenerator : MonoBehaviour
    {
        // Retained for serialized references in protected scenes.
        [SerializeField, HideInInspector] public DungeonGenerationConfig Config;
        [SerializeField, HideInInspector] public TextAsset ConfigAsset;
        [SerializeField] public GenerationConfig GenerationConfig;
        [SerializeField] public string GenerationKey = "Default";
        [SerializeField] public bool ShowLabel;
        [SerializeField] public bool ShowRoomBorders;
        [SerializeField] public bool ClearConsole;
    }
}
