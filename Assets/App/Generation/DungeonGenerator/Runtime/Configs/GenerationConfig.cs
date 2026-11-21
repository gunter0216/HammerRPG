using System;
using App.Common.Configs.External;
using App.Generation.DungeonGenerator.Runtime.Configs.Settings;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs
{
    [CreateAssetMenu(fileName = "GenerationConfig", menuName = "Autumn/Dungeon/Generation Config")]
    public class GenerationConfig : GameConfig
    {
        public const string Address = "GenerationConfig";

        [SerializeField]
        private DungeonGenerationConfig[] m_Generations = Array.Empty<DungeonGenerationConfig>();

        public DungeonGenerationConfig[] Generations => m_Generations;
    }
}
