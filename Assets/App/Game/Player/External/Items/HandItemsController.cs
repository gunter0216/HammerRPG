using App.Game.Equipment.Runtime.Config.Model;
using App.Game.Equipment.Runtime.Constants;
using App.Game.Modules.Equipment.Runtime;
using App.Game.Modules.Equipment.Runtime.Config;
using App.Game.Player.External.Context;
using UnityEngine;

namespace App.Game.Player.External.Items
{
    public class HandItemsController
    {
        private static readonly int WeaponEquipped = Animator.StringToHash("WeaponEquipped");

        private readonly PlayerContext _context;
        private Animator _animator;
        private EquipmentModule _equipment;

        public HandItemsController(PlayerContext context)
        {
            _context = context;
        }

        public OneHandedWeaponModuleConfig OneHandedWeapon { get; private set; }

        public bool Initialize()
        {
            _animator = _context.View.Animator;
            if (_animator == null || _animator.runtimeAnimatorController == null ||
                _context.RigProvider?.HasWeaponRenderers != true)
                return false;

            if (_context.ModuleItem.TryGetModule<EquipmentModule>(out _equipment))
                _equipment.SlotChanged += OnSlotChanged;

            RefreshWeapon();
            return true;
        }

        private void OnSlotChanged(EquipmentSlotId slot)
        {
            if (slot is EquipmentSlotId.RightHand or EquipmentSlotId.LeftHand)
                RefreshWeapon();
        }

        private void RefreshWeapon()
        {
            OneHandedWeapon = GetOneHandedWeapon(EquipmentSlotId.RightHand) ??
                              GetOneHandedWeapon(EquipmentSlotId.LeftHand);

            _context.RigProvider.SetWeaponSprites(OneHandedWeapon?.NearSprite, OneHandedWeapon?.FarSprite);
            _animator.SetFloat(WeaponEquipped, OneHandedWeapon != null ? 1f : 0f);
        }

        private OneHandedWeaponModuleConfig GetOneHandedWeapon(EquipmentSlotId slot)
        {
            if (_equipment == null || !_equipment.TryGetEquippedItem(slot, out var item) ||
                !item.TryGetConfigModule<EquipmentItemModuleConfig>(out var equipmentItem) ||
                equipmentItem.Type != EquipmentTypeConstants.OneHanded ||
                !item.TryGetConfigModule<OneHandedWeaponModuleConfig>(out var weapon) ||
                weapon.NearSprite == null || weapon.FarSprite == null)
                return null;

            return weapon;
        }
    }
}
