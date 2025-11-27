using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Menu.UI.Runtime.View.Panels
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button m_SingleplayerButton;
        [SerializeField] private Button m_MultiplayerButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_ExitButton;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
        
        public void SubscribeToSingleplayerButtonClick(UnityAction action)
        {
            m_SingleplayerButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeToSingleplayerButtonClick(UnityAction action)
        {
            m_SingleplayerButton.onClick.RemoveListener(action);
        }
        
        public void SubscribeToMultiplayerButtonClick(UnityAction action)
        {
            m_MultiplayerButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeToMultiplayerButtonClick(UnityAction action)
        {
            m_MultiplayerButton.onClick.RemoveListener(action);
        }
        
        public void SubscribeToSettingsButtonClick(UnityAction action)
        {
            m_SettingsButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeToSettingsButtonClick(UnityAction action)
        {
            m_SettingsButton.onClick.RemoveListener(action);
        }
        
        public void SubscribeToExitButtonClick(UnityAction action)
        {
            m_ExitButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeToExitButtonClick(UnityAction action)
        {
            m_ExitButton.onClick.RemoveListener(action);
        }
    }
}