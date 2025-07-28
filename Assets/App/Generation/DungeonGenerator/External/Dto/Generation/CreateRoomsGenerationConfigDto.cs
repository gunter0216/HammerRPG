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
        [SerializeField, JsonProperty("roomsAmount")]
        private int m_CountRooms = 20;

        [SerializeField, JsonProperty("minRoomSize")]
        private SizeIntDto m_MinRoomSize;

        [SerializeField, JsonProperty("maxRoomSize")]
        private SizeIntDto m_MaxRoomSize;

        [SerializeField, JsonProperty("shape")]
        private int m_Radius = 1;

        public int CountRooms => m_CountRooms;

        public SizeIntDto MinRoomSize => m_MinRoomSize;

        public SizeIntDto MaxRoomSize => m_MaxRoomSize;

        public int Radius => m_Radius;
    }
}