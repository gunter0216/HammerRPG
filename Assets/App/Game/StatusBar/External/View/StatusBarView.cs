using UnityEngine;
using UnityEngine.UI;

namespace App.Game.StatusBar.External.View
{
    public class StatusBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform _healthBar;

        private float? _healthBarWidth;
        
        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }

        public void SetHealth(float current, float max)
        {
            _healthBarWidth ??= _healthBar.rect.width;
            
            var percentage = current / max;
            _healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percentage * _healthBarWidth.Value);
        }
    }
}