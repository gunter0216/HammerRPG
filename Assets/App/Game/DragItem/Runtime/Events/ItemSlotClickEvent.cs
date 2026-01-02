using App.Game.DragItem.External.Model;

namespace App.Game.DragItem.External.Events
{
    public readonly struct ItemSlotClickEvent
    {
        private readonly IItemSlotModel m_Model;

        public IItemSlotModel Model => m_Model;

        public ItemSlotClickEvent(IItemSlotModel model)
        {
            m_Model = model;
        }
    }
}