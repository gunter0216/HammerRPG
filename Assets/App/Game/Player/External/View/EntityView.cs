using UnityEngine;

namespace App.Game.Player.External.View
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private Transform m_Transform;
        [SerializeField] private Rigidbody2D m_PlayerRigidbody;
        [SerializeField] private Transform m_WeaponRoot;
        [SerializeField] private SpriteRenderer m_Weapon;
        
        public int Entity { get; set; }
        
        public Transform Transform => m_Transform;
        public Rigidbody2D PlayerRigidbody => m_PlayerRigidbody;
        public Transform WeaponRoot => m_WeaponRoot;
        public SpriteRenderer Weapon => m_Weapon;
    }
}