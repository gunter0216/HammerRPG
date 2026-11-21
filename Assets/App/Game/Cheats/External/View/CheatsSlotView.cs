using UnityEngine;

namespace App.Game.Cheats.External.View
{
    public class CheatsSlotView : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button m_Button;

        [SerializeField]
        private UnityEngine.UI.Image m_Image;

        [SerializeField] private TMPro.TMP_Text m_Label;
        
        public void SetButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(callback);
        }

        public void SetSprite(Sprite sprite)
        {
            m_Image.sprite = sprite;
            m_Image.enabled = sprite != null;
        }

        public void SetLabel(string text)
        {
            m_Label.text = text;
        }

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }
    }
}
