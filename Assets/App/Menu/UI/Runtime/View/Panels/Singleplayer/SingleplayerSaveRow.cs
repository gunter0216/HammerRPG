using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Menu.UI.External.View.Panels.Singleplayer
{
    public class SingleplayerSaveRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_CreateDateText;
        [SerializeField] private TMP_Text m_NameText;
        [SerializeField] private TMP_Text m_LastLoginText;
        [SerializeField] private Button m_PlayButton;

        public void SetCreateDate(string text)
        {
            m_CreateDateText.text = text;
        }
        
        public void SetNameText(string text)
        {
            m_NameText.text = text;
        }
        
        public void SetLastLoginText(string text)
        {
            m_LastLoginText.text = text;
        }
        
        public void SetPlayButtonAction(UnityAction action)
        {
            m_PlayButton.onClick.RemoveAllListeners();
            m_PlayButton.onClick.AddListener(action);
        }
    }
}