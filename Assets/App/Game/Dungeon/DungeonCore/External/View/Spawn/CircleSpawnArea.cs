using UnityEngine;
using UnityEngine.AI;

namespace App.Game.Dungeon.DungeonCore.External.View.Spawn
{
    public class CircleSpawnArea : MonoBehaviour, ISpawnArea
    {
        [SerializeField] private float _radius = 5f;

        [Header("NavMesh")]
        [SerializeField] private float _sampleDistance = 2f;
        [SerializeField] private int _maxAttempts = 15;

        public float Weight => Mathf.PI * _radius * _radius;

        public bool TryGetSpawnPoint(out Vector3 point)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                Vector2 random =
                    Random.insideUnitCircle * _radius;

                Vector3 world =
                    transform.position +
                    new Vector3(random.x, 0f, random.y);

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
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
#endif
    }
}