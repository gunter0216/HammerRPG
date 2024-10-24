using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Menu.UI.External.View.Panels.Singleplayer
{
    public class CreateGamePanel : MonoBehaviour
    {
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Button m_CreateButton;
        [SerializeField] private TMP_InputField m_NameInputField;
        
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
        
        public string GetName()
        {
            return m_NameInputField.text;
        }
        
        public void SetBackButtonAction(UnityAction action)
        {
            m_BackButton.onClick.RemoveAllListeners();
            m_BackButton.onClick.AddListener(action);
        }
        
        public void SetCreateButtonAction(UnityAction action)
        {
            m_CreateButton.onClick.RemoveAllListeners();
            m_CreateButton.onClick.AddListener(action);
        }
    }
}