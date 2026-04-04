using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Chest
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class KeyContainerData : IContainerData
    {
        public static string ContainerKey => "KeyContainerData";
        
        [JsonProperty("data")] 
        private List<KeyModuleData> m_Data;

        IList IContainerData.Data => m_Data;
        
        public List<KeyModuleData> Data
        {
            get => m_Data;
            set => m_Data = value;
        }

        public KeyContainerData()
        {
            m_Data = new List<KeyModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(KeyContainerData);
        }
    }
}