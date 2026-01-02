using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class ItemView : MonoBehaviour
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
        
        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
        }

        public void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, 1f);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}