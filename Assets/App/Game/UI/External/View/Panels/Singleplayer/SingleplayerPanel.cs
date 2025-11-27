using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Menu.UI.Runtime.View.Panels.Singleplayer
{
    public class SingleplayerPanel : MonoBehaviour
    {
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Button m_RemoveButton;
        [SerializeField] private Button m_CreateNewGameButton;
        [SerializeField] private Transform m_SaveRowsContent;
        [SerializeField] private SingleplayerSaveRow m_SaveRowPrefab;

        public SingleplayerSaveRow SaveRowPrefab => m_SaveRowPrefab;
        public Transform SaveRowsContent => m_SaveRowsContent;
        
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
        
        public void SetRemoveButtonAction(UnityAction action)
        {
            m_RemoveButton.onClick.RemoveAllListeners();
            m_RemoveButton.onClick.AddListener(action);
        }
        
        public void SetCreateNewGameButtonAction(UnityAction action)
        {
            m_CreateNewGameButton.onClick.RemoveAllListeners();
            m_CreateNewGameButton.onClick.AddListener(action);
        }
        
        public void SubscribeToBackButtonClick(UnityAction action)
        {
            m_BackButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeToBackButtonClick(UnityAction action)
        {
            m_BackButton.onClick.RemoveListener(action);
        }
    }
}