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
        [SerializeField, JsonProperty("Rooms Amount")]
        private int m_CountRooms = 20;

        [SerializeField, JsonProperty("Min Room Size")]
        private SizeIntDto m_MinRoomSize;

        [SerializeField, JsonProperty("Max Room Size")]
        private SizeIntDto m_MaxRoomSize;

        [SerializeField, JsonProperty("Shape")]
        private int m_Radius = 1;

        public int CountRooms => m_CountRooms;

        public SizeIntDto MinRoomSize => m_MinRoomSize;

        public SizeIntDto MaxRoomSize => m_MaxRoomSize;

        public int Radius => m_Radius;
    }
}