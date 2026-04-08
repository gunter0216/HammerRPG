using TMPro;
using UnityEngine;

namespace App.Game.CharacterWindow.External.View
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statText;
        [SerializeField] private TMP_Text _valueText;

        public void SetStatName(string text)
        {
            _statText.text = text;
        }
        
        public void SetStatValue(string text)
        {
            _valueText.text = text;
        }
    }
}