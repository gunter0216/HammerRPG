using System;
using App.Game.DungeonGenerator.Runtime.Rooms;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators
{
    [Serializable]
    public class DungeonConfig
    {
        [Header("Base")]
        [SerializeField] private int m_Width = 128;
        [SerializeField] private int m_Height = 96;
        [Header("Rooms")]
        [SerializeField] private DungeonRoomsConfig m_Rooms = new();

        public int Width
        {
            get => m_Width;
            set => m_Width = value;
        }
        
        public int Height
        {
            get => m_Height;
            set => m_Height = value;
        }
        
        public DungeonRoomsConfig Rooms
        {
            get => m_Rooms;
            set => m_Rooms = value;
        }
    }
}