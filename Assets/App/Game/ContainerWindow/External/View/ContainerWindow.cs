using App.Game.DragItem.Runtime.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.ContainerWindow.External.View
{
    public class ContainerWindow : MonoBehaviour
    {
        [SerializeField] private ItemSlotView m_ItemSlotViewPrefab;
        [SerializeField] private Transform m_SlotsContent;
        [SerializeField] private TMP_Text _nameText;
        [Space]
        [SerializeField] private Button m_CloseButton;
        
        public ItemSlotView ItemSlotViewPrefab => m_ItemSlotViewPrefab;
        public Transform SlotsContent => m_SlotsContent;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }

        public void SetName(string text)
        {
            _nameText.text = text;
        }
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(callback);
        }
    }
}