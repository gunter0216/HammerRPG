using App.Common.ModuleItem.Runtime;

namespace App.Game.Player.External.Items
{
    public readonly struct EquipHandItemInfo
    {
        private readonly EHand _hand;
        private readonly IModuleItem _moduleItem;

        public EquipHandItemInfo(EHand hand, IModuleItem moduleItem)
        {
            _hand = hand;
            _moduleItem = moduleItem;
        }

        public EHand Hand => _hand;
        public IModuleItem Item => _moduleItem;
    }
}