using System;
using App.Generation.DungeonGenerator.External.Dto.Common;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition
{
    [Serializable]
    public class SquarePartitionGenerationConfig : IGenerationConfig
    {
        private readonly SizeIntDto _size;
        private readonly float _minPartition;
        private readonly float _maxPartition;
        private readonly float _offset;
        private readonly int _minAreaSize;
        private readonly int _minRoomSize;
        private readonly int _maxRoomSize;
        private readonly int _areaPadding;
        private readonly int _depth;

        public SizeIntDto Size => _size;
        public int MinAreaSize => _minAreaSize;

        public int MinRoomSize => _minRoomSize;

        public float MinPartition => _minPartition;

        public float MaxPartition => _maxPartition;

        public int Depth => _depth;

        public float Offset => _offset;

        public int MaxRoomSize => _maxRoomSize;

        public int AreaPadding => _areaPadding;

        public SquarePartitionGenerationConfig(SizeIntDto size,
            float minPartition,
            float maxPartition,
            int minAreaSize,
            int minRoomSize, 
            int maxRoomSize,
            int depth, 
            float offset, 
            int areaPadding)
        {
            _size = size;
            _minPartition = minPartition;
            _maxPartition = maxPartition;
            _minAreaSize = minAreaSize;
            _minRoomSize = minRoomSize;
            _depth = depth;
            _offset = offset;
            _maxRoomSize = maxRoomSize;
            _areaPadding = areaPadding;
        }
    }
}