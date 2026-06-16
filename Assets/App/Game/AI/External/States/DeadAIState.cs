using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.AI.External.States
{
    public class DeadAIState : IAIState
    {
        private static readonly int _die = Animator.StringToHash("Die");
        
        private readonly EntityView _entityView;
        private readonly Animator _animator;

        public DeadAIState(EntityView entityView)
        {
            _entityView = entityView;
            _animator = _entityView.Animator;
        }

        public void Enter()
        {
            _animator.SetBool(_die, true);
        }

        public void Exit()
        {
            _animator.SetBool(_die, false);
        }
    }
}