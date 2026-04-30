using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Level.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class LevelContainerData : IContainerData
    {
        public static string ContainerKey => "LevelContainerData";
        
        [JsonProperty("data")] 
        private List<LevelModuleData> m_Data;

        IList IContainerData.Data => m_Data;

        public LevelContainerData()
        {
            m_Data = new List<LevelModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(LevelContainerData);
        }
    }
}