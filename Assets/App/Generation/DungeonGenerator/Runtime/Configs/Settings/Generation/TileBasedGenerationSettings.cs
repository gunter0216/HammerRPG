using System;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation
{
    [Serializable]
    public class TileBasedGenerationSettings
    {
        [SerializeField] private DungeonRoomConfig[] _rooms = Array.Empty<DungeonRoomConfig>();
        [SerializeField] private int _maxDepth = 2;
        [SerializeField] private int _maxIterations = 300;
        [SerializeField, InspectorName("Запретить вращение комнат")] private bool _disableRoomRotation;

        public DungeonRoomConfig[] Rooms => _rooms;
        public int MaxDepth => _maxDepth;
        public int MaxIterations => _maxIterations;
        public bool DisableRoomRotation => _disableRoomRotation;
    }
}
