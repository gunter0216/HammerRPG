using UnityEngine;

namespace App.Game.Player.External.View
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private Transform _weaponRoot;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _attackAnimation;
        [Header("Attack")]
        [SerializeField] private Vector3 _boxSize = new(2f, 1f, 2f);
        [SerializeField] private Vector3 _boxOffset = Vector3.zero;
        [SerializeField] private float _attackDistance = 1.5f;
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Debug")]
        [SerializeField] private bool _showGizmos;

        public Vector3 BoxSize => _boxSize;
        public float AttackDistance => _attackDistance;
        public LayerMask EnemyLayer => _enemyLayer;

        public Vector3 GetAttackCenter()
        {
            return transform.position + transform.forward * _attackDistance;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_showGizmos)
            {
                return;
            }

            Gizmos.color = Color.red;

            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(
                GetAttackCenter(),
                transform.rotation,
                Vector3.one);

            Gizmos.DrawWireCube(_boxOffset, _boxSize);

            Gizmos.matrix = oldMatrix;
        }
        
        public int Entity { get; set; }
        
        public Transform Transform => transform;
        public Rigidbody PlayerRigidbody => _playerRigidbody;
        public Transform WeaponRoot => _weaponRoot;
        public Animator Animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = gameObject.GetComponentInChildren<Animator>();
                }

                return _animator;
            }
        }

        public AnimationClip AttackAnimation => _attackAnimation;
    }
}