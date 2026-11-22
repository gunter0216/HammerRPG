using UnityEngine;
using UnityEngine.UI;

namespace App.Game.DragItem.Runtime.View
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Image m_Image;
        private Canvas m_Canvas;
        private Vector2 _defaultIconSize;
        private bool _defaultPreserveAspect;

        private void Awake()
        {
            m_Canvas = GetComponentInParent<Canvas>();
            _defaultIconSize = m_Image.rectTransform.rect.size;
            _defaultPreserveAspect = m_Image.preserveAspect;
        }

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
        
        public void SetIcon(Sprite sprite, UnityEngine.UI.Image sourceIcon = null)
        {
            m_Image.sprite = sprite;
            var iconRect = m_Image.rectTransform;
            var size = _defaultIconSize;
            var preserveAspect = _defaultPreserveAspect;
            if (sourceIcon != null)
            {
                var sourceRect = sourceIcon.rectTransform;
                var sourceSize = sourceIcon.GetPixelAdjustedRect().size;
                var width = sourceRect.TransformVector(Vector3.right * sourceSize.x);
                var height = sourceRect.TransformVector(Vector3.up * sourceSize.y);
                size = new Vector2(iconRect.InverseTransformVector(width).magnitude,
                    iconRect.InverseTransformVector(height).magnitude);
                preserveAspect = sourceIcon.preserveAspect;
            }
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            m_Image.preserveAspect = preserveAspect;
        }
        
        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
        }

        public void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, 1f);
        }

        public bool TrySetScreenPosition(Vector2 position)
        {
            var parent = m_RectTransform.parent as RectTransform;
            if (m_Canvas == null)
                m_Canvas = GetComponentInParent<Canvas>();
            if (parent == null || m_Canvas == null)
                return false;

            var canvas = m_Canvas.rootCanvas;
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, position, camera, out var worldPosition))
                return false;

            m_RectTransform.position = worldPosition;
            return true;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}
