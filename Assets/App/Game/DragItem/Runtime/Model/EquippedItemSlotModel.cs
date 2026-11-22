using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Modules.Equipment.Runtime;
using App.Game.Modules.Equipment.Runtime.Config;

namespace App.Game.DragItem.Runtime.Model
{
    public sealed class EquippedItemSlotModel : IItemSlotModel
    {
        private readonly EquipmentModule _equipment;
        private readonly EquipmentSlotId _slot;

        public EquippedItemSlotModel(EquipmentModule equipment, EquipmentSlotId slot, UnityEngine.UI.Image icon)
        {
            _equipment = equipment;
            _slot = slot;
            Icon = icon;
        }

        public UnityEngine.UI.Image Icon { get; }

        public IModuleItem GetItem() => _equipment.TryGetEquippedItem(_slot, out var item) ? item : null;

        public bool CanPlaceItem(IModuleItem item) => _equipment.CanPlaceItem(_slot, item);

        public bool PlaceItem(IModuleItem item) => _equipment.TryEquip(_slot, item);

        public Optional<IModuleItem> RemoveItem() => _equipment.TryUnequip(_slot, out var item)
            ? Optional<IModuleItem>.Success(item)
            : Optional<IModuleItem>.Fail();

        public bool CanMoveToInventory(ItemSlotController target) =>
            _equipment.CanUnequipToInventory(_slot, target.Container, target.Index);

        public bool TryMoveToInventory(ItemSlotController target) =>
            _equipment.TryUnequipToInventory(_slot, target.Container, target.Index);

        public void UpItem() => SetIconAlpha(0.5f);

        public void DownItem() => SetIconAlpha(1f);

        private void SetIconAlpha(float alpha)
        {
            var color = Icon.color;
            color.a = alpha;
            Icon.color = color;
        }
    }
}
