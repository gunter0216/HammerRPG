using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Equipment.External.View
{
    public class EquipmentWindow : MonoBehaviour
    {
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private EquipmentSlotsView m_SlotsView;

        public EquipmentSlotsView SlotsView => m_SlotsView;
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(callback);
        }
        
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
