using UnityEngine;

namespace App.Game.GameManagers.External.Room
{
    public class RoomView : IRoomView
    {
        private readonly Transform m_Content;

        public Transform Content => m_Content;
        
        public void SetParent(Transform parent)
        {
            m_Content.SetParent(parent);
        }

        public RoomView(Transform content)
        {
            m_Content = content;
        }
    }
}