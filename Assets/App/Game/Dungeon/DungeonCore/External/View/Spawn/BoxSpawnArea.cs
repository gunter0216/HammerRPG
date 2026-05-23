using UnityEngine;
using UnityEngine.AI;

namespace App.Game.Dungeon.DungeonCore.External.View.Spawn
{
    public class BoxSpawnArea : MonoBehaviour, ISpawnArea
    {
        [SerializeField] private Vector2 _size = new(5f, 5f);

        [Header("NavMesh")]
        [SerializeField] private float _sampleDistance = 2f;
        [SerializeField] private int _maxAttempts = 15;

        public float Weight => _size.x * _size.y;

        public bool TryGetSpawnPoint(out Vector3 point)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                Vector3 local = new(
                    Random.Range(-_size.x * 0.5f, _size.x * 0.5f),
                    0f,
                    Random.Range(-_size.y * 0.5f, _size.y * 0.5f));

                Vector3 world = transform.TransformPoint(local);

                if (NavMesh.SamplePosition(
                        world,
                        out NavMeshHit hit,
                        _sampleDistance,
                        NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }

            point = default;
            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Matrix4x4 old = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireCube(
                Vector3.zero,
                new Vector3(_size.x, 0.1f, _size.y));

            Gizmos.matrix = old;
        }
#endif
    }
}