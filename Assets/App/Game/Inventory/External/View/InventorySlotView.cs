using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Button m_Button;
        
        public void SetButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(callback);
        }

        public Vector2 GetLocalPosition()
        {
            return m_RectTransform.localPosition;
        }
    }
}