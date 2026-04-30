using App.Game.DragItem.Runtime.View;
using App.Game.Inventory.External.View;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Equipment.External.View
{
    public class EquipmentItemSlotView : MonoBehaviour, IItemSlotView
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Image m_Item;
        [SerializeField] private Image m_Placeholder;
        [SerializeField] private Button m_Button;

        public void SetItemActive(bool status)
        {
            m_Placeholder.gameObject.SetActive(!status);
            m_Item.gameObject.SetActive(status);
        }
        
        public void SetSprite(Sprite sprite)
        {
            m_Item.sprite = sprite;
        }

        public void SetPickupState(bool status)
        {
            var color = m_Item.color;
            color.a = status ? 0.5f : 1f;
            m_Item.color = color;
        }
        
        public void SetButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(callback);
        }
    }
}