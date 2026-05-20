using System;
using UnityEngine;

namespace App.Game.Player.External.View
{
    public class AttackAnimationEventListener : MonoBehaviour
    {
        public event Action OnAttackEvent;
        
        public void OnAttack()
        {
            OnAttackEvent?.Invoke();
        }
    }
}