using UnityEngine;

namespace App.Game.Player.Runtime.Events
{
    public struct AttackEvent
    {
        private int m_EntityId;
        private Vector3 m_Position;

        public int EntityId => m_EntityId;
        public Vector3 Position => m_Position;

        public AttackEvent(int entityId, Vector3 position)
        {
            m_EntityId = entityId;
            m_Position = position;
        }
    }
}