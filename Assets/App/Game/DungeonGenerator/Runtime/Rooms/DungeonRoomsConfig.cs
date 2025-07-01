using System;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.Rooms
{
    [Serializable]
    public class DungeonRoomsConfig
    {
        [SerializeField] private int m_CountRooms = 20;
        
        [SerializeField] private int m_MinWidthRoom = 7;
        [SerializeField] private int m_MaxWidthRoom = 12;
        [SerializeField] private int m_MinHeightRoom = 7;
        [SerializeField] private int m_MaxHeightRoom = 12;
        
        [SerializeField] private int m_HeightRoomThreshold = 9;
        [SerializeField] private int m_WidthRoomThreshold = 9;        
        
        [SerializeField] private int m_MinCorridorSize = 10;
        
        [SerializeField] private int m_Radius = 1;
        
        public int CountRooms
        {
            get => m_CountRooms;
            set => m_CountRooms = value;
        }
        
        public int MinWidthRoom
        {
            get => m_MinWidthRoom;
            set => m_MinWidthRoom = value;
        }
        
        public int MaxWidthRoom
        {
            get => m_MaxWidthRoom;
            set => m_MaxWidthRoom = value;
        }
        
        public int MinHeightRoom
        {
            get => m_MinHeightRoom;
            set => m_MinHeightRoom = value;
        }
        
        public int MaxHeightRoom
        {
            get => m_MaxHeightRoom;
            set => m_MaxHeightRoom = value;
        }
        
        public int RoomsRadius
        {
            get => m_Radius;
            set => m_Radius = value;
        }
        
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
        
        public int MinCorridorSize
        {
            get => m_MinCorridorSize;
            set => m_MinCorridorSize = value;
        }
    }
}