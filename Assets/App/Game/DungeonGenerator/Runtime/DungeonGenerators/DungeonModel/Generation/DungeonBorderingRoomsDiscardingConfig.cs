using System;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    [Serializable]
    public class DungeonBorderingRoomsDiscardingConfig
    {
        [SerializeField] private int m_MinCorridorSize = 10;
        
        public int MinCorridorSize
        {
            get => m_MinCorridorSize;
            set => m_MinCorridorSize = value;
        }
    }
}