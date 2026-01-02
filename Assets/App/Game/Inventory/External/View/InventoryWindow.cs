using App.Game.DragItem.Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private InventoryGroupHeaderView m_InventoryGroupHeaderViewPrefab;
        [SerializeField] private Transform m_HeaderGroupContent;
        [Space]
        [SerializeField] private ItemSlotView m_ItemSlotViewPrefab;
        [SerializeField] private Transform m_SlotsContent;
        [Space]
        [SerializeField] private Transform m_ItemsContent;
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private Button m_BlockButton;
        
        public InventoryGroupHeaderView InventoryGroupHeaderViewPrefab => m_InventoryGroupHeaderViewPrefab;
        public Transform HeaderGroupContent => m_HeaderGroupContent;
        public ItemSlotView ItemSlotViewPrefab => m_ItemSlotViewPrefab;
        public Transform SlotsContent => m_SlotsContent;
        public Transform ItemsContent => m_ItemsContent;
        
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