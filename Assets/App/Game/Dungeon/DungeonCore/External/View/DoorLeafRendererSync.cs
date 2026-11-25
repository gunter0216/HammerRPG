using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.View
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class DoorLeafRendererSync : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _secondLeaf;

        private SpriteRenderer _firstLeaf;

        private void OnEnable()
        {
            _firstLeaf = GetComponent<SpriteRenderer>();
            Synchronize();

#if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
                UnityEditor.EditorApplication.update += Synchronize;
#endif
        }

#if UNITY_EDITOR
        private void OnDisable() => UnityEditor.EditorApplication.update -= Synchronize;
#endif

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
                return;
#endif
            Synchronize();
        }

        private void Synchronize()
        {
            if (_firstLeaf == null || _secondLeaf == null)
                return;

            // Room instances already override the first renderer's flip and sorting.
            if (_secondLeaf.flipX != _firstLeaf.flipX)
                _secondLeaf.flipX = _firstLeaf.flipX;
            if (_secondLeaf.sortingLayerID != _firstLeaf.sortingLayerID)
                _secondLeaf.sortingLayerID = _firstLeaf.sortingLayerID;
            if (_secondLeaf.sortingOrder != _firstLeaf.sortingOrder)
                _secondLeaf.sortingOrder = _firstLeaf.sortingOrder;
        }
    }
}
