using App.Game.AI.External.View;

namespace App.Game.AI.External.States
{
    internal sealed class RunAIState2D : IAIState, IUpdateState
    {
        private readonly EnemyCombat2D _combat;

        public RunAIState2D(EnemyCombat2D combat)
        {
            _combat = combat;
        }

        public void Enter()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            _combat.MoveToTarget();
        }

        public void Exit()
        {
            _combat.StopMovement();
        }
    }
}
