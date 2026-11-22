using App.Common.ModuleItem.Runtime.Config;
using App.Game.Equipment.Runtime.Config.Model;
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

        [Header("Melee (2D)")]
        [SerializeField, Min(0)] private float _swingInnerOffset = 0.25f;
        [SerializeField] private float _swingHeight = 0.9f;
        [SerializeField] private LayerMask _swingObstacleLayers = ~0;

        [Header("Debug")]
        [SerializeField] private bool _showGizmos;
        [SerializeField] private ModuleItemGameConfig _previewWeapon;

        public Vector3 BoxSize => _boxSize;
        public LayerMask EnemyLayer => _enemyLayer;
        internal LayerMask SwingObstacleLayers => _swingObstacleLayers;
        
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

        public Vector3 GetAttackCenter(bool facingLeft = false)
        {
            var offset = _boxOffset;
            if (facingLeft)
                offset.x = -offset.x;

            return transform.position + transform.rotation * offset;
        }

        internal (Vector2 Origin, Vector2 Center, Vector2 Size, float Angle) GetSwingArea2D(
            OneHandedWeaponModuleConfig weapon, bool facingLeft)
        {
            float depth = Mathf.Max(0, weapon.Reach - _swingInnerOffset);
            float side = facingLeft ? -1f : 1f;
            Vector2 origin = transform.TransformPoint(0, _swingHeight, 0);
            Vector2 center = transform.TransformPoint(side * (_swingInnerOffset + depth * 0.5f), _swingHeight, 0);
            var size = new Vector2(transform.TransformVector(Vector3.right * depth).magnitude,
                transform.TransformVector(Vector3.up * weapon.SweepWidth).magnitude);
            return (origin, center, size, transform.eulerAngles.z);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_showGizmos)
            {
                return;
            }

            if (_previewWeapon != null && _previewWeapon.Modules != null &&
                _previewWeapon.TryGetModule<OneHandedWeaponModuleConfig>(out var weapon))
            {
                DrawSwingArea2D(weapon, false);
                DrawSwingArea2D(weapon, true);
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

        private void DrawSwingArea2D(OneHandedWeaponModuleConfig weapon, bool facingLeft)
        {
            var (_, center, size, angle) = GetSwingArea2D(weapon, facingLeft);
            var oldMatrix = Gizmos.matrix;
            var oldColor = Gizmos.color;
            Gizmos.matrix = Matrix4x4.TRS(new Vector3(center.x, center.y, transform.position.z),
                Quaternion.Euler(0, 0, angle), Vector3.one);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, size.y, 0));
            Gizmos.matrix = oldMatrix;
            Gizmos.color = oldColor;
        }
#endif
    }
}
