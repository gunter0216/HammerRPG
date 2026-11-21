using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.Player.External.Attack
{
    public class PlayerMeleeAttackState : StateMachineBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _normalizedContactTime;

        private bool _contactSent;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _contactSent = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SendContact(animator, stateInfo.normalizedTime);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SendContact(animator, stateInfo.normalizedTime);
            if (animator.TryGetComponent<AttackAnimationEventListener>(out var listener))
                listener.NotifyAttackFinished();
        }

        private void SendContact(Animator animator, float normalizedTime)
        {
            if (_contactSent || normalizedTime < _normalizedContactTime)
                return;

            _contactSent = true;
            if (animator.TryGetComponent<AttackAnimationEventListener>(out var listener))
                listener.OnAttack();
        }
    }
}
