using App.Game.Cheats.External.View;
using App.Game.Inventory.External.View;

namespace App.Game.Cheats.External.ViewModel
{
    public class CheatsSlotViewModel
    {
        private readonly CheatsSlotView m_View;

        public CheatsSlotViewModel(CheatsSlotView view)
        {
            m_View = view;
        }
    }
}