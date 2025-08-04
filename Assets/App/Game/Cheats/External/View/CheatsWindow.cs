using App.Game.Inventory.External.View;
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
        private Transform m_ItemsContent;

        public CheatsGroupHeaderView CheatsGroupHeaderViewPrefab => m_CheatsGroupHeaderViewPrefab;
        public Transform HeaderGroupContent => m_HeaderGroupContent;
        public CheatsSlotView CheatsSlotViewPrefab => m_CheatsSlotViewPrefab;
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
    }
}