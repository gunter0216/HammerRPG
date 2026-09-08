using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Level.Runtime.Config
{
    public class LevelModuleConfig : ModuleConfig
    {
        [SerializeField] private int _startLevel;

        public int StartLevel => _startLevel;

        public LevelModuleConfig(int startLevel)
        {
            _startLevel = startLevel;
        }
    }
}