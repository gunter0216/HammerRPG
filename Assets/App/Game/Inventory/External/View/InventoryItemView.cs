using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Image m_Image;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void SetSize(float width, float height)
        {
            m_RectTransform.sizeDelta = new Vector2(width, height);
        }
        
        public void SetLocalPosition(Vector2 position)
        {
            m_RectTransform.localPosition = position;
        }
        
        public void SetIcon(Sprite sprite)
        {
            m_Image.sprite = sprite;
        }
    }
}