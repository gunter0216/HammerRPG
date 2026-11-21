using System;
using App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings
{
    [Serializable]
    public class DungeonGenerationConfig
    {
        [SerializeField]
        private string m_Key;
        
        [SerializeField, FormerlySerializedAs("_tileTileBasedGenerationConfigDto")]
        private TileBasedGenerationSettings _tileBasedGenerationConfig;

        public string Key => m_Key;

        public TileBasedGenerationSettings TileBasedGenerationConfig => _tileBasedGenerationConfig;
    }
}
