namespace App.Game.Player.External.Items
{
    public interface IItemController
    {
        void Equip(EquipHandItemInfo info);
        void UnEquip(UnEquipHandItemInfo info);
    }
}