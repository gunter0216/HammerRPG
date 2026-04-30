using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.GameMenu.External.View
{
    public class HUDView : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private Image _healthImage;
        [SerializeField] private TMP_Text _healthText;
        [Header("Mana")]
        [SerializeField] private Image _manaImage;
        [SerializeField] private TMP_Text _manaText;
        [Header("Buttons")]
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _characterButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _questsButton;
        
        private float _healthWidth = 0;
        
        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }

        public void SetInventoryButtonClickCallback(UnityAction action)
        {
            _inventoryButton.onClick.RemoveListener(action);
            _inventoryButton.onClick.AddListener(action);
        }
        
        public void SetCharacterButtonClickCallback(UnityAction action)
        {
            _characterButton.onClick.RemoveListener(action);
            _characterButton.onClick.AddListener(action);
        }
        
        public void SetSettingsButtonClickCallback(UnityAction action)
        {
            _settingsButton.onClick.RemoveListener(action);
            _settingsButton.onClick.AddListener(action);
        }
        
        public void SetQuestsButtonClickCallback(UnityAction action)
        {
            _questsButton.onClick.RemoveListener(action);
            _questsButton.onClick.AddListener(action);
        }
        
        public void SetHealth(int currentValue, int maxValue)
        {
            if (_healthWidth == 0)
            {
                _healthWidth = _healthImage.rectTransform.rect.width;
            }

            var width = ((float)currentValue / maxValue) * _healthWidth;
            _healthText.text = $"{currentValue}/{maxValue}";
            _healthImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}