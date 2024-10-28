using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.GameMenu.Runtime.View
{
    public class GameMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_ExitButton;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
        
        public void SetContinueButtonClickAction(UnityAction action)
        {
            m_ContinueButton.onClick.RemoveAllListeners();
            m_ContinueButton.onClick.AddListener(action);
        }
        
        public void SetSettingsButtonClickAction(UnityAction action)
        {
            m_SettingsButton.onClick.RemoveAllListeners();
            m_SettingsButton.onClick.AddListener(action);
        }
        
        public void SetExitButtonClickAction(UnityAction action)
        {
            m_ExitButton.onClick.RemoveAllListeners();
            m_ExitButton.onClick.AddListener(action);
        }
    }
}