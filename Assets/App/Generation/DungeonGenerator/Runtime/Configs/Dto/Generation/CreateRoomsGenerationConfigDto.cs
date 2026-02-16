using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class CreateRoomsGenerationConfigDto
    {
        [JsonProperty("roomsAmount")]
        private int m_CountRooms = 20;

        [JsonProperty("minRoomSize")]
        private SizeIntDto m_MinRoomSize;

        [JsonProperty("maxRoomSize")]
        private SizeIntDto m_MaxRoomSize;

        [JsonProperty("shape")]
        private int m_Radius = 1;

        public int CountRooms => m_CountRooms;

        public SizeIntDto MinRoomSize => m_MinRoomSize;

        public SizeIntDto MaxRoomSize => m_MaxRoomSize;

        public int Radius => m_Radius;
    }
}