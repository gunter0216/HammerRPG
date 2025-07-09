using System;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    [Serializable]
    public class DungeonConfig
    {
        [Header("Base")]
        [SerializeField] private int m_Width = 128;
        [SerializeField] private int m_Height = 96;
        [Header("Rooms")]
        [SerializeField] private DungeonRoomsCreateConfig m_RoomsCreate = new();
        [SerializeField] private DungeonSmallRoomsDiscardingConfig m_SmallRooms = new();
        [SerializeField] private DungeonBorderingRoomsDiscardingConfig m_BorderingRooms = new();
        [SerializeField] private DungeonSeparationConfig m_SeparationConfig = new();

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
        
        public DungeonRoomsCreateConfig RoomsCreate
        {
            get => m_RoomsCreate;
            set => m_RoomsCreate = value;
        }

        public DungeonSmallRoomsDiscardingConfig SmallRooms
        {
            get => m_SmallRooms;
            set => m_SmallRooms = value;
        }

        public DungeonBorderingRoomsDiscardingConfig BorderingRooms
        {
            get => m_BorderingRooms;
            set => m_BorderingRooms = value;
        }

        public DungeonSeparationConfig SeparationConfig
        {
            get => m_SeparationConfig;
            set => m_SeparationConfig = value;
        }
    }
}