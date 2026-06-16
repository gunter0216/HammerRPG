using UnityEngine;

namespace App.Game.Player.External.View
{
    public class MeleeAttackView : MonoBehaviour
    {
        [SerializeField] private AttackAnimationEventListener _attackAnimationEventListener;
        [SerializeField] private AnimationClip _attackAnimation;
        [SerializeField] private float _attackDistance = 2f;
        [Header("Attack")]
        [SerializeField] private Vector3 _boxSize = new(2f, 1f, 2f);
        [SerializeField] private Vector3 _boxOffset = Vector3.zero;
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Debug")]
        [SerializeField] private bool _showGizmos;

        public Vector3 BoxSize => _boxSize;
        public LayerMask EnemyLayer => _enemyLayer;
        
        public Transform Transform => transform;
        
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

        public float AttackDistance => _attackDistance;

        public Vector3 GetAttackCenter()
        {
            return transform.position + transform.rotation * _boxOffset;
        }

#if UNITY_EDITOR
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
#endif
    }
}