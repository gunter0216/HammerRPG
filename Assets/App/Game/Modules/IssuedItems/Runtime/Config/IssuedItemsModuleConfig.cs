using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.IssuedItems.Runtime.Config
{
    [Serializable]
    public class IssuedItemsModuleConfig : ModuleConfig, ISerializationCallbackReceiver
    {
        [Serializable]
        public sealed class Entry
        {
            [SerializeField] private ModuleItemGameConfig _item;
            [SerializeField, Min(1)] private int _quantity = 1;

            public ModuleItemGameConfig Item => _item;
            public int Quantity => _quantity;

            public Entry(ModuleItemGameConfig item, int quantity = 1)
            {
                _item = item;
                _quantity = quantity;
            }
        }

        [SerializeField, Tooltip("Предметы, выдаваемые в ContainerModule игрока на старте.")]
        private List<Entry> _entries = new();

        [SerializeField, HideInInspector]
        private List<ModuleItemGameConfig> _items;

        public IReadOnlyList<Entry> Items => _entries;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_items == null || _items.Count == 0)
                return;

            // Legacy configurations listed one item reference for each copy.
            _entries ??= new List<Entry>(_items.Count);
            if (_entries.Count == 0)
            {
                foreach (var item in _items)
                    _entries.Add(new Entry(item));
            }

            _items = null;
        }
    }
}
