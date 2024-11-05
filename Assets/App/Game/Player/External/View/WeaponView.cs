using System;
using UnityEngine;
using UnityEngine.Events;

namespace App.Game.Player.External.View
{
    public class WeaponView : MonoBehaviour
    {
        private UnityAction<Collider2D> m_OnCollisionEnter2DAction;
        
        public void SetOnTriggerEnter2D(UnityAction<Collider2D> action)
        {
            m_OnCollisionEnter2DAction = action;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            m_OnCollisionEnter2DAction?.Invoke(other);
        }
    }
}