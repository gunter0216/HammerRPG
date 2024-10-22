using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Menu.UI.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameRecord
    {
        [SerializeField, JsonProperty("Name")] private string m_Name;
        [SerializeField, JsonProperty("DateOfCreation")] private long m_DateOfCreation;
        [SerializeField, JsonProperty("LastLogin")] private long m_LastLogin;

        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }
        
        public long DateOfCreation
        {
            get => m_DateOfCreation;
            set => m_DateOfCreation = value;
        }
        
        public long LastLogin
        {
            get => m_LastLogin;
            set => m_LastLogin = value;
        }
    }
}