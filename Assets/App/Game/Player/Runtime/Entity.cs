using App.Common.Time.Runtime;

namespace App.Game.Player.Runtime
{
    public struct Entity
    {
        public float MoveSpeed;
        public RealtimeTimer AttackTimer;
        public EntityView View;
    }
}