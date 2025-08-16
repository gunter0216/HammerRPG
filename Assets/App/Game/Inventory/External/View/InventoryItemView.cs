using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Image m_Image;
        [SerializeField] private Button m_Button;

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
        
        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
        }

        public void SetButtonClickCallback(UnityAction callback)
        {
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(callback);
        }
        
        public void SetButtonActive(bool status)
        {
            m_Button.gameObject.SetActive(status);
        }

        public void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}