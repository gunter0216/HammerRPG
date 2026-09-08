using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model
{
    public class GameItemTypeModuleConfig : ModuleConfig
    {
        [SerializeField] private string m_Type;

        public string Type => m_Type;

        public GameItemTypeModuleConfig(string type)
        {
            m_Type = type;
        }
    }
}