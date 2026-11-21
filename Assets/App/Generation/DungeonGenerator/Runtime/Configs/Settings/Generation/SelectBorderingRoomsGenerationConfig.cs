using System;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation
{
    [Serializable]
    public class SelectBorderingRoomsGenerationConfig
    {
        [SerializeField]
        private int m_MinCorridorSize = 3;

        public int MinCorridorSize => m_MinCorridorSize;
    }
}