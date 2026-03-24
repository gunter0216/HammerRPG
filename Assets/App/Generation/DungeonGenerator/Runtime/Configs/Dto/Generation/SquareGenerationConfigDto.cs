using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SquareGenerationConfigDto
    {
        [JsonProperty("size")] [SerializeField]
        private SizeIntDto _size;
        
        [JsonProperty("offset")] [SerializeField] [Range(0, 1)]
        private float _offset;
        
        [JsonProperty("min_partition")] [SerializeField] [Range(0, 1)]
        private float _minPartition;
        
        [JsonProperty("max_partition")] [SerializeField] [Range(0, 1)]
        private float _maxPartition;
        
        [JsonProperty("min_area_size")] [SerializeField]
        private int _minAreaSize;
        
        [JsonProperty("min_room_size")] [SerializeField]
        private int _minRoomSize;
        
        [JsonProperty("max_room_size")] [SerializeField]
        private int _maxRoomSize;
        
        [JsonProperty("max_room_size")] [SerializeField]
        private int _areaPadding;
        
        [JsonProperty("depth")] [SerializeField]
        private int _depth;
        
        public SizeIntDto Size => _size;

        public int MinAreaSize => _minAreaSize;

        public int MinRoomSize => _minRoomSize;

        public float MinPartition => _minPartition;

        public float MaxPartition => _maxPartition;
        public int Depth => _depth;
        public float Offset => _offset;
        public int MaxRoomSize => _maxRoomSize;
        public int AreaPadding => _areaPadding;
    }
}