using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.States.Game;

namespace App.Game.Cheats.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class CheatsController : IInitSystem
    {
        public void Init()
        {
            
        }
    }
}