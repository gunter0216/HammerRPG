using App.Game.Inventory.External.View;
using TMPro;
using UnityEngine;

namespace App.Game.Cheats.External.View
{
    public class CheatsWindow : MonoBehaviour
    {
        [SerializeField]
        private CheatsGroupHeaderView m_CheatsGroupHeaderViewPrefab;
        [SerializeField]
        private Transform m_HeaderGroupContent;
        [Space]
        [SerializeField]
        private CheatsSlotView m_CheatsSlotViewPrefab;
        [SerializeField]
        private Transform m_SlotsContent;
        [Space]
        [SerializeField]
        private TMP_Dropdown m_CreateItemDropdown;

        public CheatsGroupHeaderView CheatsGroupHeaderViewPrefab => m_CheatsGroupHeaderViewPrefab;
        public Transform HeaderGroupContent => m_HeaderGroupContent;
        public CheatsSlotView CheatsSlotViewPrefab => m_CheatsSlotViewPrefab;
        public Transform SlotsContent => m_SlotsContent;
        public TMP_Dropdown CreateItemDropdown => m_CreateItemDropdown;

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