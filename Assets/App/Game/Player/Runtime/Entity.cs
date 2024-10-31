using App.Common.Time.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Player.Runtime
{
    public struct Entity
    {
        public Transform Transform;
        public Rigidbody2D PlayerRigidbody;
        public float MoveSpeed;
        public bool IsAttacking;
        public RealtimeTimer AttackTimer;
        public Image WeaponImage;
    }
}