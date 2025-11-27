using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Menu.UI.Runtime.Data;
using Newtonsoft.Json;

namespace App.Menu.UI.External.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameRecordsData : IData, IGameRecordsData
    {
        [JsonProperty("GameRecords")] private List<GameRecord> m_GameRecords;
        
        public List<GameRecord> GameRecords
        {
            get => m_GameRecords;
            set => m_GameRecords = value;
        }
        
        public string Name()
        {
            return nameof(GameRecordsData);
        }
    }
}