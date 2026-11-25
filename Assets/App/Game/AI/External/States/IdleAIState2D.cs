using App.Game.AI.External.View;

namespace App.Game.AI.External.States
{
    internal sealed class IdleAIState2D : IAIState
    {
        private readonly EnemyCombat2D _combat;

        public IdleAIState2D(EnemyCombat2D combat)
        {
            _combat = combat;
        }

        public void Enter()
        {
            _combat.StopMovement();
        }

        public void Exit()
        {
        }
    }
}
