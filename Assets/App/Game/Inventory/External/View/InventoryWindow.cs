using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private InventoryGroupHeaderView m_InventoryGroupHeaderViewPrefab;
        [SerializeField] private Transform m_HeaderGroupContent;
        [Space]
        [SerializeField] private InventorySlotView m_InventorySlotViewPrefab;
        [SerializeField] private Transform m_SlotsContent;
        [Space]
        [SerializeField] private Transform m_ItemsContent;
        [SerializeField] private InventoryItemView m_InventoryItemViewPrefab;
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private Button m_BlockButton;
        [SerializeField] private Transform m_SelectedItemContent;
        
        public InventoryGroupHeaderView InventoryGroupHeaderViewPrefab => m_InventoryGroupHeaderViewPrefab;
        public Transform HeaderGroupContent => m_HeaderGroupContent;
        public InventorySlotView InventorySlotViewPrefab => m_InventorySlotViewPrefab;
        public Transform SlotsContent => m_SlotsContent;
        public Transform ItemsContent => m_ItemsContent;
        public InventoryItemView InventoryItemViewPrefab => m_InventoryItemViewPrefab;
        public Transform SelectedItemContent => m_SelectedItemContent;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
        
        public void SetBlockButtonActive(bool isActive)
        {
            m_BlockButton.gameObject.SetActive(isActive);
        }
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(callback);
        }
        
        public void SetBlockButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_BlockButton.onClick.RemoveAllListeners();
            m_BlockButton.onClick.AddListener(callback);
        }
    }
}