using System;
using App.Common.ModuleItem.Runtime.Data;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using Newtonsoft.Json;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Chest
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class KeyModuleData : IModuleData
    {
        [JsonProperty("data")]
        private DungeonKeyData _data;

        public KeyModuleData()
        {
        }
        
        public KeyModuleData(DungeonKeyData data)
        {
            _data = data;
        }
        

        public DungeonKeyData Data => _data;

        public string GetModuleKey()
        {
            return KeyContainerData.ContainerKey;
        }
    }
}