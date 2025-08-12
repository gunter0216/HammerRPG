using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace App.Common.Utilities.UtilityUnity.Runtime
{
    public class TextList : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> m_Texts;
        
        public string Text
        {
            set => SetText(value);
        }

        public void SetText(string text)
        {
            foreach (var tmpText in m_Texts)
            {
                tmpText.text = text;
            }
        }
    }
}