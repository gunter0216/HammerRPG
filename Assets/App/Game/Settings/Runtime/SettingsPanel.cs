﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Game.Settings.Runtime
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Button m_BackButton;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
        
        public void SubscribeToBackButtonClick(UnityAction action)
        {
            m_BackButton.onClick.AddListener(action);
        }
        
        public void UnSubscribeToBackButtonClick(UnityAction action)
        {
            m_BackButton.onClick.RemoveListener(action);
        }
    }
}