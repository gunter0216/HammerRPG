using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.AI.External.States
{
    public class RunAIState : IAIState
    {
        private static readonly int _run = Animator.StringToHash("Run");
        
        private readonly EntityView _entityView;
        private readonly Animator _animator;

        public RunAIState(EntityView entityView)
        {
            _entityView = entityView;
            _animator = _entityView.Animator;
        }

        public void Enter()
        {
            _animator.SetBool(_run, true);
        }

        public void Exit()
        {
            _animator.SetBool(_run, false);
        }
    }
}