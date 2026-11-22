using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using App.Common.Events.Runtime;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;

namespace App.Game.DragItem.Runtime.View
{
    public class ItemSlotView : MonoBehaviour, IItemSlotView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform m_RectTransform;
        [SerializeField] private Image m_Item;
        [SerializeField] private UnityEngine.UI.Image m_ItemBackground;
        [SerializeField] private UnityEngine.UI.Image m_DropPreview;
        [SerializeField] private Button m_Button;

        private RectTransform _visualLayer;
        private RectTransform _iconRoot;
        private RectTransform _previewRoot;
        private Color _restingBackgroundColor;

        private static readonly Color HoverBackgroundColor = new Color32(65, 64, 70, 255);

        public UnityEngine.UI.Image Icon => m_Item;
        public ItemSlotController Model { get; private set; }

        public void Bind(ItemSlotController model)
        {
            Model = model;
            if (m_ItemBackground != null)
                _restingBackgroundColor = m_ItemBackground.color;
            InitializeVisualLayers();
        }

        public void SetItemHovered(bool hovered)
        {
            if (m_ItemBackground != null)
                m_ItemBackground.color = hovered ? HoverBackgroundColor : _restingBackgroundColor;
        }

        private void InitializeVisualLayers()
        {
            if (_iconRoot != null)
                return;

            var grid = m_RectTransform.parent;
            _visualLayer = GetOrCreateLayer(grid, "ItemVisuals");
            var layoutElement = _visualLayer.GetComponent<UnityEngine.UI.LayoutElement>();
            if (layoutElement == null)
                layoutElement = _visualLayer.gameObject.AddComponent<UnityEngine.UI.LayoutElement>();
            layoutElement.ignoreLayout = true;

            var previews = GetOrCreateLayer(_visualLayer, "Previews");
            var icons = GetOrCreateLayer(_visualLayer, "Icons");
            icons.SetAsLastSibling();
            _iconRoot = CreateVisualRoot(icons, m_Item.rectTransform);
            if (m_DropPreview != null)
                _previewRoot = CreateVisualRoot(previews, m_DropPreview.rectTransform);
            _visualLayer.SetAsLastSibling();
        }

        private static RectTransform GetOrCreateLayer(Transform parent, string name)
        {
            var layer = parent.Find(name) as RectTransform;
            if (layer != null)
                return layer;

            layer = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            layer.gameObject.layer = parent.gameObject.layer;
            layer.SetParent(parent, false);
            layer.anchorMin = Vector2.zero;
            layer.anchorMax = Vector2.one;
            layer.offsetMin = Vector2.zero;
            layer.offsetMax = Vector2.zero;
            return layer;
        }

        private RectTransform CreateVisualRoot(Transform layer, RectTransform graphic)
        {
            var root = new GameObject(name + "_" + graphic.name, typeof(RectTransform)).GetComponent<RectTransform>();
            root.gameObject.layer = gameObject.layer;
            root.SetParent(layer, false);
            root.pivot = m_RectTransform.pivot;
            graphic.SetParent(root, false);
            SyncVisualRoot(root);
            root.gameObject.SetActive(isActiveAndEnabled);
            return root;
        }

        private void SyncVisualRoot(RectTransform root)
        {
            if (root == null)
                return;

            root.SetPositionAndRotation(m_RectTransform.position, m_RectTransform.rotation);
            root.localScale = m_RectTransform.localScale;
            var size = m_RectTransform.rect.size;
            root.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            root.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }

        private void LateUpdate()
        {
            if (m_Item.gameObject.activeSelf)
                SyncVisualRoot(_iconRoot);
            if (m_DropPreview != null && m_DropPreview.gameObject.activeSelf)
                SyncVisualRoot(_previewRoot);
        }

        private void OnEnable()
        {
            if (_iconRoot != null)
                _iconRoot.gameObject.SetActive(true);
            if (_previewRoot != null)
                _previewRoot.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (_iconRoot != null)
                _iconRoot.gameObject.SetActive(false);
            if (_previewRoot != null)
                _previewRoot.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_iconRoot != null)
                Destroy(_iconRoot.gameObject);
            if (_previewRoot != null)
                Destroy(_previewRoot.gameObject);
        }

        public void SetDropPreview(bool active, int width = 1, int height = 1)
        {
            if (m_DropPreview == null)
                return;

            if (active)
            {
                SyncVisualRoot(_previewRoot);
                var layout = m_RectTransform.parent.GetComponent<UnityEngine.UI.GridLayoutGroup>();
                SetFootprint(m_DropPreview.rectTransform, width, height,
                    layout != null ? layout.spacing : Vector2.zero, 1);
            }
            m_DropPreview.gameObject.SetActive(active);
        }

        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
            if (_visualLayer != null)
                _visualLayer.SetAsLastSibling();
        }
        
        public void SetItemActive(bool status)
        {
            m_Item.gameObject.SetActive(status);
            if (m_ItemBackground != null)
                m_ItemBackground.gameObject.SetActive(status);
        }
        
        public void SetSprite(Sprite sprite)
        {
            m_Item.sprite = sprite;
        }

        public void SetFootprint(int width, int height)
        {
            SyncVisualRoot(_iconRoot);
            var layout = m_RectTransform.parent.GetComponent<UnityEngine.UI.GridLayoutGroup>();
            var spacing = layout != null ? layout.spacing : Vector2.zero;
            SetFootprint(m_Item.rectTransform, width, height, spacing, 0);
            if (m_ItemBackground != null)
                SetFootprint(m_ItemBackground.rectTransform, width, height, spacing, 1);
            m_Item.preserveAspect = width > 1 || height > 1;
        }

        private static void SetFootprint(RectTransform rect, int width, int height, Vector2 spacing, float inset)
        {
            rect.anchorMin = new Vector2(0, 1 - height);
            rect.anchorMax = new Vector2(width, 1);
            rect.offsetMin = new Vector2(inset, -spacing.y * (height - 1) + inset);
            rect.offsetMax = new Vector2(spacing.x * (width - 1) - inset, -inset);
        }

        public Vector2 GetCellPointerOffset(Vector2 screenPoint)
        {
            var canvas = GetComponentInParent<Canvas>()?.rootCanvas;
            var camera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
            var rect = m_RectTransform.rect;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, screenPoint, camera, out var local) ||
                !rect.Contains(local))
                return Vector2.zero;

            var layout = m_RectTransform.parent.GetComponent<UnityEngine.UI.GridLayoutGroup>();
            var step = rect.size + (layout != null ? layout.spacing : Vector2.zero);
            var offset = local - rect.center;
            return new Vector2(offset.x / step.x, -offset.y / step.y);
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || Model == null)
                return;

            eventData.eligibleForClick = false;
            EventManager.Trigger(new ItemSlotDragStartedEvent(this, Model, eventData.pointerId));
        }

        // The drag controller updates the cursor icon and placement preview together each frame.
        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || Model == null)
                return;

            EventManager.Trigger(new ItemSlotDragEndedEvent(this, eventData));
        }
    }
}
