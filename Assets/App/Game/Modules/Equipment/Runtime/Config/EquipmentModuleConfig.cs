using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.Equipment.Runtime.Constants;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace App.Game.Modules.Equipment.Runtime.Config
{
    [Serializable]
    [MovedFrom(true, sourceClassName: "EquipmentSlotsModuleConfig")]
    public sealed class EquipmentModuleConfig : ModuleConfig, ISerializationCallbackReceiver
    {
        [Serializable]
        public sealed class SlotRule
        {
            [SerializeField] private EquipmentSlotId _slot = EquipmentSlotId.Helmet;
            [SerializeField] private EquipmentType[] _allowedEquipmentTypes = Array.Empty<EquipmentType>();

            [FormerlySerializedAs("_allowedTypes"), SerializeField, HideInInspector]
            private string[] _legacyAllowedTypes;

            public EquipmentSlotId Slot => _slot;
            public IReadOnlyList<EquipmentType> AllowedTypes => _allowedEquipmentTypes;

            internal void MigrateLegacyTypes()
            {
                if (_legacyAllowedTypes == null || _legacyAllowedTypes.Length == 0 || _allowedEquipmentTypes?.Length > 0)
                    return;

                var types = new EquipmentType[_legacyAllowedTypes.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    if (!EquipmentTypeConstants.TryGetType(_legacyAllowedTypes[i], out types[i]))
                        return;
                }

                _allowedEquipmentTypes = types;
                _legacyAllowedTypes = null;
            }
        }

        [SerializeField] private List<SlotRule> _slots = new();

        public IReadOnlyList<SlotRule> Slots => _slots;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_slots == null)
                return;

            foreach (var rule in _slots)
                rule?.MigrateLegacyTypes();
        }
    }
}
