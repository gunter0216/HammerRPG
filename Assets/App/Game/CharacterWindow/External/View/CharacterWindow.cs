using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.CharacterWindow.External.View
{
    public class CharacterWindow : MonoBehaviour
    {
        [SerializeField] private Image _personIcon; 
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _raceText;
        [Space]
        [SerializeField] private Image _experienceImage;
        [SerializeField] private TMP_Text _experienceText;
        [SerializeField] private TMP_Text _levelText;
        [Space]
        [SerializeField] private MainStatView _strengthStat;
        [SerializeField] private MainStatView _agilityStat;
        [SerializeField] private MainStatView _intelligenceStat;
        // [SerializeField] private MainStatView _mainStatViewPrefab;
        // [SerializeField] private Transform _mainStatsContent;
        [Space]
        [SerializeField] private StatView _statViewPrefab;
        [SerializeField] private Transform _statsContent;
        [Space]
        [SerializeField] private Button _closeButton;
        
        // public MainStatView MainStatViewPrefab => _mainStatViewPrefab;
        // public Transform MainStatsContent => _mainStatsContent;
        public StatView StatViewPrefab => _statViewPrefab;
        public Transform StatsContent => _statsContent;
        
        public MainStatView StrengthStat => _strengthStat;
        public MainStatView AgilityStat => _agilityStat;
        public MainStatView IntelligenceStat => _intelligenceStat;

        public void SetPersonIcon(Sprite icon)
        {
            _personIcon.sprite = icon;
        }
        
        public void SetName(string text)
        {
            _nameText.text = text;
        }
        
        public void SetRace(string text)
        {
            _raceText.text = text;
        }
        
        public void SetExperience(int currentValue, int maxValue)
        {
            _experienceText.text = $"{currentValue}/{maxValue}";
            _experienceImage.fillAmount = (float)currentValue / maxValue;
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }

        public void SetLevel(int level)
        {
            _levelText.text = level.ToString();
        }
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(callback);
        }
    }
}