using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Player.Runtime
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private Transform m_Transform;
        [SerializeField] private Rigidbody2D m_PlayerRigidbody;
        [SerializeField] private SpriteRenderer m_Weapon;
        
        public Transform Transform => m_Transform;
        public Rigidbody2D PlayerRigidbody => m_PlayerRigidbody;
        public SpriteRenderer Weapon => m_Weapon;
    }
}