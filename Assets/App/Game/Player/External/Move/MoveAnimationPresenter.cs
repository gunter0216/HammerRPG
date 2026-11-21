using App.Game.Player.External.Context;
using UnityEngine;

namespace Game.Project.Gameplay.Move.External.View
{
    public class MoveAnimationPresenter
    {
        private static readonly int IsWalkingLeft = Animator.StringToHash("IsWalkingLeft");
        private static readonly int IsWalkingRight = Animator.StringToHash("IsWalkingRight");

        private readonly Transform _transform;
        private readonly PlayerMoveContext _context;

        private Animator _animator;

        public MoveAnimationPresenter(Transform transform, PlayerMoveContext context)
        {
            _transform = transform;
            _context = context;
        }

        public void Initialize()
        {
            _animator = _transform.GetComponentInChildren<Animator>();
        }

        public void OnUpdate(Vector2 velocity, bool isAttacking)
        {
            if (_animator == null || _animator.runtimeAnimatorController == null)
                return;

            bool facingLeft = _context.IsFacingLeft;
            if (Mathf.Abs(velocity.x) > 0.01f)
            {
                facingLeft = velocity.x < 0f;
                if (!isAttacking)
                    _context.IsFacingLeft = facingLeft;
            }

            var isWalking = velocity.sqrMagnitude > 0.0001f;
            _animator.SetBool(IsWalkingLeft, isWalking && facingLeft);
            _animator.SetBool(IsWalkingRight, isWalking && !facingLeft);
        }
    }
}
