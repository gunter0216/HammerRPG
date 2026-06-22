namespace App.Game.Player.External.Items
{
    public interface IHandItemsController
    {
        void Equip(EquipHandItemInfo info);
        void UnEquip(UnEquipHandItemInfo info);
    }
}