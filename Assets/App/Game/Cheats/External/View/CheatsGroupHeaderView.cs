using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.Cheats.External.View
{
    public class CheatsGroupHeaderView : MonoBehaviour
    {
        [SerializeField] private Image m_Background;
        [SerializeField] private Image m_Icon;
        [SerializeField] private Button m_Button;

        public void SetActiveStatus(bool status)
        {
            var multiplier = status ? 0.6f : 1.0f;
            m_Background.color = new Color(multiplier, multiplier, multiplier);
        }
        
        public void SetIcon(Sprite sprite)
        {
            m_Icon.sprite = sprite;
        }
        
        public void SetButtonClickCallback(UnityAction callback)
        {
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(callback);
        }
    }
}