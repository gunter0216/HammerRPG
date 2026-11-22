using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace App.Game.Equipment.Runtime.Config.Model
{
    [System.Serializable]
    [MovedFrom(true, sourceClassName: "EquipmentModuleConfig")]
    public class EquipmentItemModuleConfig : ModuleConfig
    {
        [SerializeField] private string m_Type;

        public string Type => m_Type;

        public EquipmentItemModuleConfig(string type)
        {
            m_Type = type;
        }
    }
}
