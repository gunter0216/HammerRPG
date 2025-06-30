using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.Pause.Runtime;
using App.Game.States.Game;
using UnityEngine;

namespace App.Game.Pause.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class PauseController : IPauseController
    {
        public void Pause()
        {
            // todo конечно тут будет не тайм скэйл xd, возможно
            Time.timeScale = 0;
        }

        public void Unpause()
        {
            Time.timeScale = 1;
        }
    }
}