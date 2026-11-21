using System;
using App.Generation.DungeonGenerator.Runtime.Configs.Settings.Common;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation
{
    [Serializable]
    public class SelectSmallRoomsGenerationConfig
    {
        [SerializeField]
        private SizeInt m_RoomThreshold;

        public SizeInt RoomThreshold => m_RoomThreshold;
    }
}
