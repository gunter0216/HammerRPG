using System;
using App.Generation.DungeonGenerator.Runtime.Configs.Settings.Common;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Generation
{
    [Serializable]
    public class SquareGenerationConfig
    {
        [SerializeField]
        private SizeInt _size;
        
        [SerializeField] [Range(0, 1)]
        private float _offset;
        
        [SerializeField]
        private int _minAreaSize;
        
        [SerializeField]
        private int _minRoomSize;
        
        [SerializeField]
        private int _maxRoomSize;
        
        [SerializeField]
        private int _areaPadding;
        
        [SerializeField]
        private int _depth;
        
        public SizeInt Size => _size;

        public int MinAreaSize => _minAreaSize;

        public int MinRoomSize => _minRoomSize;

        public int Depth => _depth;
        public float Offset => _offset;
        public int MaxRoomSize => _maxRoomSize;
        public int AreaPadding => _areaPadding;
    }
}