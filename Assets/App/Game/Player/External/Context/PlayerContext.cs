using App.Common.ModuleItem.Runtime;
using App.Game.Player.External.View;

namespace App.Game.Player.External.Context
{
    public class PlayerContext
    {
        // contexts
        public PlayerMoveContext MoveContext { get; } = new();
        public PlayerAttackContext AttackContext { get; } = new();
        
        // modules
        public IModuleItem ModuleItem { get; set; }
        
        // view
        public EntityView View { get; set; }
        public RigProviderView RigProvider { get; set; }
    }
}