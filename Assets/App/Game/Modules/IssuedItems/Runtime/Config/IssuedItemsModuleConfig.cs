using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.IssuedItems.Runtime.Config
{
    [Serializable]
    public class IssuedItemsModuleConfig : ModuleConfig
    {
        [SerializeField, Tooltip("Предметы, выдаваемые в ContainerModule игрока на старте.")]
        private List<ModuleItemGameConfig> _items = new();

        public IReadOnlyList<ModuleItemGameConfig> Items => _items;
    }
}
