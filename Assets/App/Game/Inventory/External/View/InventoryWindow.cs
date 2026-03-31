using App.Game.DragItem.Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private ItemSlotView _itemSlotViewPrefab;
        [SerializeField] private Transform _slotsContent;
        [Space]
        [SerializeField] private Transform _itemsContent;
        [SerializeField] private Button _closeButton;
        
        public ItemSlotView ItemSlotViewPrefab => _itemSlotViewPrefab;
        public Transform SlotsContent => _slotsContent;
        public Transform ItemsContent => _itemsContent;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(callback);
        }
    }
}