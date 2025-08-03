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
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}