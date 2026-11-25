using App.Game.AI.External.View;

namespace App.Game.AI.External.States
{
    internal sealed class AttackAIState2D : IAIState, IUpdateState
    {
        private readonly EnemyCombat2D _combat;

        public AttackAIState2D(EnemyCombat2D combat)
        {
            _combat = combat;
        }

        public void Enter()
        {
            _combat.StopMovement();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_combat.IsAttacking)
                return;

            if (!_combat.TryAttack())
                return;
        }

        public void Exit()
        {
            _combat.CancelAttack();
        }
    }
}
