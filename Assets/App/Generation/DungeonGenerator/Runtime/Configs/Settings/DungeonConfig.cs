using System;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings
{
    [Serializable]
    public class DungeonConfig
    {
        [SerializeField]
        private DungeonGenerationConfig m_Generation;

        public DungeonGenerationConfig Generation => m_Generation;
    }
}
