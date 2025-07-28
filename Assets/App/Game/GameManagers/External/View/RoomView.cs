using UnityEngine;

namespace App.Game.GameManagers.External.View
{
    public class RoomView
    {
        private readonly Transform m_Content;

        public Transform Content => m_Content;

        public RoomView(Transform content)
        {
            m_Content = content;
        }
    }
}