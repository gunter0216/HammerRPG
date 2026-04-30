using UnityEngine;

namespace App.Game.Equipment.External.View
{
    public class EquipmentSlotsView : MonoBehaviour
    {
        [SerializeField] private EquipmentItemSlotView m_Helmet;
        [SerializeField] private EquipmentItemSlotView m_Armor;
        [SerializeField] private EquipmentItemSlotView m_Shoulder;
        [SerializeField] private EquipmentItemSlotView m_Gloves;
        [SerializeField] private EquipmentItemSlotView m_Amulet;
        [SerializeField] private EquipmentItemSlotView m_Bracer;
        [SerializeField] private EquipmentItemSlotView m_Ring1;
        [SerializeField] private EquipmentItemSlotView m_Ring2;
        [SerializeField] private EquipmentItemSlotView m_Belt;
        [SerializeField] private EquipmentItemSlotView m_Pants;
        [SerializeField] private EquipmentItemSlotView m_Boots;
        [SerializeField] private EquipmentItemSlotView m_LeftHand;
        [SerializeField] private EquipmentItemSlotView m_RightHand;

        public EquipmentItemSlotView Helmet => m_Helmet;
        public EquipmentItemSlotView Armor => m_Armor;
        public EquipmentItemSlotView Shoulder => m_Shoulder;
        public EquipmentItemSlotView Gloves => m_Gloves;
        public EquipmentItemSlotView Amulet => m_Amulet;
        public EquipmentItemSlotView Bracer => m_Bracer;
        public EquipmentItemSlotView Ring1 => m_Ring1;
        public EquipmentItemSlotView Ring2 => m_Ring2;
        public EquipmentItemSlotView Belt => m_Belt;
        public EquipmentItemSlotView Pants => m_Pants;
        public EquipmentItemSlotView Boots => m_Boots;
        public EquipmentItemSlotView LeftHand => m_LeftHand;
        public EquipmentItemSlotView RightHand => m_RightHand;
    }
}