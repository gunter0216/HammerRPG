using System;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.ContainerItem.Runtime.Config
{
    [Serializable]
    public class ContainerItemModuleConfig : ModuleConfig
    {
        [SerializeField, Min(1), Tooltip("Ширина предмета в ячейках инвентаря.")]
        private int _width = 1;

        [SerializeField, Min(1), Tooltip("Высота предмета в ячейках инвентаря.")]
        private int _height = 1;

        public int Width => _width;
        public int Height => _height;
        public bool IsValid => _width > 0 && _height > 0;
    }
}
