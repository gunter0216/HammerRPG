using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Name.Runtime.Config
{
    public class NameModuleConfig : ModuleConfig
    {
        [SerializeField] private string _name;

        public string Name => _name;

        public NameModuleConfig(string name)
        {
            _name = name;
        }
    }
}