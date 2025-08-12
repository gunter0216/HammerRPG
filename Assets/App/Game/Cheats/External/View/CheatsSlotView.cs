using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Cheats.External.View
{
    public class CheatsSlotView : MonoBehaviour
    {
        [SerializeField] private Button m_Button;

        [SerializeField]
        private Image m_Image;
        
        public void SetButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(callback);
        }

        public void SetSprite(Sprite sprite)
        {
            m_Image.sprite = sprite;
        }

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }
    }
}