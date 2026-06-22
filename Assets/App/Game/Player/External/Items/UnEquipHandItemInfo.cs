namespace App.Game.Player.External.Items
{
    public readonly struct UnEquipHandItemInfo
    {
        private readonly EHand _hand;

        public UnEquipHandItemInfo(EHand hand)
        {
            _hand = hand;
        }

        public EHand Hand => _hand;
    }
}