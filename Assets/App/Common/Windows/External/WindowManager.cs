using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.States.Runtime.Game;

namespace App.Common.Windows.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class WindowManager : IWindowManager
    {
        
    }
}