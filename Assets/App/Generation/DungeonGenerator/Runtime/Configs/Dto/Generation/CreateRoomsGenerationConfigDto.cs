using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class CreateRoomsGenerationConfigDto
    {
        [JsonProperty("roomsAmount")] [SerializeField]
        private int m_CountRooms = 20;

        [JsonProperty("minRoomSize")] [SerializeField]
        private SizeIntDto m_MinRoomSize;

        [JsonProperty("maxRoomSize")] [SerializeField]
        private SizeIntDto m_MaxRoomSize;

        [JsonProperty("shape")] [SerializeField]
        private int m_Radius = 1;

        public int CountRooms => m_CountRooms;

        public SizeIntDto MinRoomSize => m_MinRoomSize;

        public SizeIntDto MaxRoomSize => m_MaxRoomSize;

        public int Radius => m_Radius;
    }
}