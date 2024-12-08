using App.Common.Timer.Runtime;
using App.Game.Player.External.View;

namespace App.Game.Player.Runtime.Components
{
    public struct EntityComponent
    {
        public float MoveSpeed { get; set; }
        public RealtimeTimer AttackTimer { get; set; }
        public EntityView View { get; set; }
    }
}