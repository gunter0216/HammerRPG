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
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Debug")]
        [SerializeField] private bool _showGizmos;

        private AttackAnimationEventListener _attackAnimationEventListener;

        public Vector3 BoxSize => _boxSize;
        public LayerMask EnemyLayer => _enemyLayer;

        public Vector3 GetAttackCenter()
        {
            return transform.position + _boxOffset;
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
        
        public AttackAnimationEventListener AttackAnimationEventListener
        {
            get
            {
                if (_attackAnimationEventListener == null)
                {
                    _attackAnimationEventListener = gameObject.GetComponentInChildren<AttackAnimationEventListener>();
                }

                return _attackAnimationEventListener;
            }
        }

        public AnimationClip AttackAnimation => _attackAnimation;
    }
}