using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Race.Runtime.Config
{
    public class RaceModuleConfig : ModuleConfig
    {
        [SerializeField] private ERace _race;

        public ERace Race => _race;

        public RaceModuleConfig(ERace race)
        {
            _race = race;
        }
    }
}