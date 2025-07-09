using System;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    [Serializable]
    public class DungeonSmallRoomsDiscardingConfig
    {
        [SerializeField] private int m_HeightRoomThreshold = 9;
        [SerializeField] private int m_WidthRoomThreshold = 9;        
        
        public int HeightRoomThreshold
        {
            get => m_HeightRoomThreshold;
            set => m_HeightRoomThreshold = value;
        }
        
        public int WidthRoomThreshold
        {
            get => m_WidthRoomThreshold;
            set => m_WidthRoomThreshold = value;
        }
    }
}