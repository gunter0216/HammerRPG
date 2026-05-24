using App.Game.Player.External.View;
using DG.Tweening;
using UnityEngine;

namespace App.Game.AI.External.States
{
    public class AttackAIState : IAIState, IUpdateState
    {
        private static readonly int _attack = Animator.StringToHash("Attack");

        private readonly MeleeAttackView _meleeAttackView;
        private readonly EntityView _entityView;
        private readonly Animator _animator;
        private Sequence _attackSeq;
        private bool _isAttack;
        private EntityView _target;

        public AttackAIState(EntityView entityView)
        {
            _entityView = entityView;
            _animator = _entityView.Animator;
            _meleeAttackView = _entityView.GetComponent<MeleeAttackView>();
        }

        public bool CanAttack(EntityView enemyView)
        {
            var distance = Vector3.Distance(enemyView.transform.position, _entityView.transform.position);
            if (distance <= _meleeAttackView.AttackDistance)
            {
                return true;
            }

            return false;
        }

        public void Enter()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            var transform = _entityView.Transform;
            Vector3 direction = _target.Transform.position - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * deltaTime);
        }

        public void Attack(EntityView target)
        {
            _target = target;
            _isAttack = true;
            _attackSeq?.Kill();
            _attackSeq = DOTween.Sequence().AppendInterval(_meleeAttackView.AttackAnimation.length).OnComplete(() =>
            {
                _isAttack = false;
            });
            
            _animator.SetBool(_attack, true);
        }

        public void Exit()
        {
            _attackSeq?.Kill();
            _isAttack = false;
            _animator.SetBool(_attack, false);
        }

        public bool IsAttack()
        {
            return _isAttack;
        }
    }
}