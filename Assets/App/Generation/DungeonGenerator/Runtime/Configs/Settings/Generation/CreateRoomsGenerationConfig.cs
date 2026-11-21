using System;
using App.Generation.DungeonGenerator.Runtime.Configs.Settings.Common;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation
{
    [Serializable]
    public class CreateRoomsGenerationConfig
    {
        [SerializeField]
        private int m_CountRooms = 20;

        [SerializeField]
        private SizeInt m_MinRoomSize;

        [SerializeField]
        private SizeInt m_MaxRoomSize;

        [SerializeField]
        private int m_Radius = 1;

        public int CountRooms => m_CountRooms;

        public SizeInt MinRoomSize => m_MinRoomSize;

        public SizeInt MaxRoomSize => m_MaxRoomSize;

        public int Radius => m_Radius;
    }
}