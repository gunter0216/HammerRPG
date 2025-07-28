using UnityEngine;

namespace App.Game.GameManagers.External.View
{
    public class TileView : MonoBehaviour, ITileView
    {
        [SerializeField]
        private SpriteRenderer m_SpriteRenderer;
        [SerializeField]
        private BoxCollider2D m_BoxCollider2D;

        public SpriteRenderer SpriteRenderer
        {
            get => m_SpriteRenderer;
            set => m_SpriteRenderer = value;
        }

        public void SetSprite(Sprite sprite)
        {
            m_SpriteRenderer.sprite = sprite;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }

        public void DisableCollider()
        {
            SetColliderActive(false);
        }

        public void SetColliderActive(bool status)
        {
            m_BoxCollider2D.enabled = status;
        }
    }
}