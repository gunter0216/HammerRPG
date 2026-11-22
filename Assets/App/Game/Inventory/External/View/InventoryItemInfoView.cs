using App.Common.ModuleItem.Runtime;
using App.Game.Modules.ContainerItem.Runtime;
using App.Game.Modules.Name.Runtime.Config;
using App.Game.Modules.Name.Runtime.Data;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using TMPro;
using UnityEngine;

namespace App.Game.Inventory.External.View
{
    public sealed class InventoryItemInfoView : MonoBehaviour
    {
        private const float Gap = 12f;

        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemType;
        [SerializeField] private TMP_Text _itemDetails;
        [SerializeField] private GameObject _price;
        [SerializeField] private GameObject _equippedBadge;

        private RectTransform _rect;
        private RectTransform _viewport;
        private RectTransform _badge;

        public void Initialize(RectTransform viewport)
        {
            _viewport = viewport;
            _rect = (RectTransform)transform;
            _badge = (RectTransform)_equippedBadge.transform;
            _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 132f);
            _price.SetActive(false);
            _equippedBadge.SetActive(false);
            Hide();
        }

        public void SetItem(IModuleItem item, bool equipped)
        {
            _itemName.text = GetItemName(item);
            string type = item.TryGetConfigModule<GameItemTypeModuleConfig>(out var config) ? config.Type : null;
            _itemType.text = type;
            _itemType.gameObject.SetActive(!string.IsNullOrWhiteSpace(type));
            _itemDetails.text = item.TryGetModule<ContainerItemModule>(out var size)
                ? $"Размер: {size.Width}×{size.Height}"
                : string.Empty;
            _equippedBadge.SetActive(equipped);
        }

        public bool TryPositionLeftOf(RectTransform footprint)
        {
            Vector2 itemLeft = _viewport.InverseTransformPoint(
                footprint.TransformPoint(new Vector3(footprint.rect.xMin, footprint.rect.center.y)));
            Rect viewport = _viewport.rect;
            float right = itemLeft.x - Gap;
            float availableWidth = right - viewport.xMin - Gap;
            float availableHeight = viewport.height - 2f * Gap;
            if (availableWidth <= 0f || availableHeight <= 0f)
                return false;

            Vector2 size = _rect.rect.size;
            float badgeExtension = _equippedBadge.activeSelf
                ? Mathf.Max(0f, _badge.localPosition.y + _badge.rect.yMax - _rect.rect.yMax)
                : 0f;
            float scale = Mathf.Min(1f, Mathf.Min(availableWidth / size.x, availableHeight / (size.y + badgeExtension)));
            Vector2 halfSize = size * (scale * 0.5f);
            float y = Mathf.Clamp(itemLeft.y, viewport.yMin + Gap + halfSize.y,
                viewport.yMax - Gap - halfSize.y - badgeExtension * scale);
            _rect.localScale = new Vector3(scale, scale, 1f);
            _rect.localPosition = new Vector3(right - halfSize.x, y, 0f);
            gameObject.SetActive(true);
            return true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private static string GetItemName(IModuleItem item)
        {
            if (item.TryGetDataModule<NameModuleData>(out var data) && !string.IsNullOrWhiteSpace(data.Name))
                return data.Name;
            if (item.TryGetConfigModule<NameModuleConfig>(out var config) && !string.IsNullOrWhiteSpace(config.Name))
                return config.Name;
            return item.Id.Replace('_', ' ');
        }
    }
}
