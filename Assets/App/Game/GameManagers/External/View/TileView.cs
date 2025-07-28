using UnityEngine;

namespace App.Game.GameManagers.External.View
{
    public class TileView : MonoBehaviour, ITileView
    {
        [SerializeField]
        private SpriteRenderer m_SpriteRenderer;

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
    }
}