using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.AI.External.States
{
    public class IdleAIState : IAIState
    {
        private static readonly int _idle = Animator.StringToHash("Idle");
        
        private readonly EntityView _entityView;
        private readonly Animator _animator;

        public IdleAIState(EntityView entityView)
        {
            _entityView = entityView;
            _animator = _entityView.Animator;
        }

        public void Enter()
        {
            _animator.SetBool(_idle, true);
        }

        public void Exit()
        {
            _animator.SetBool(_idle, false);
        }
    }
}