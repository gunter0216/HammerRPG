using UnityEngine;

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
        
        public InventoryGroupHeaderView InventoryGroupHeaderViewPrefab => m_InventoryGroupHeaderViewPrefab;
        public Transform HeaderGroupContent => m_HeaderGroupContent;
        public InventorySlotView InventorySlotViewPrefab => m_InventorySlotViewPrefab;
        public Transform SlotsContent => m_SlotsContent;
        public Transform ItemsContent => m_ItemsContent;
        public InventoryItemView InventoryItemViewPrefab => m_InventoryItemViewPrefab;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}