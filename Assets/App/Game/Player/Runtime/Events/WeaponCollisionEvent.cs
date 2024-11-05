using UnityEngine;

namespace App.Game.Player.Runtime.Events
{
    public struct WeaponCollisionEvent
    {
        private int m_AttackerEntityId;
        private int m_AttackedEntityId;
        private Collider2D m_Other;

        public int AttackerEntityId => m_AttackerEntityId;
        public int AttackedEntityId => m_AttackedEntityId;
        public Collider2D Other => m_Other;

        public WeaponCollisionEvent(int attackerEntityId, int attackedEntityId, Collider2D other)
        {
            m_AttackerEntityId = attackerEntityId;
            m_AttackedEntityId = attackedEntityId;
            m_Other = other;
        }
    }
}