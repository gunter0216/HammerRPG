using System;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation
{
    [Serializable]
    public class SeparateRoomsGenerationConfig
    {
        [SerializeField]
        private int m_Speed = 1;

        public int Speed => m_Speed;
    }
}