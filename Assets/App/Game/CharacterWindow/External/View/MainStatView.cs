using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.CharacterWindow.External.View
{
    public class MainStatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Button _increaseButton;

        public void SetStatName(string text)
        {
            _statText.text = text;
        }
        
        public void SetStatValue(string text)
        {
            _valueText.text = text;
        }
        
        public void SetStatValue(int value)
        {
            _valueText.text = value.ToString();
        }

        public void SetIncreaseButtonActive(bool status)
        {
            _increaseButton.gameObject.SetActive(status);
        }
        
        public void SetIncreaseButtonCallback(UnityAction action)
        {
            _increaseButton.onClick.RemoveAllListeners();
            _increaseButton.onClick.AddListener(action);
        }
    }
}