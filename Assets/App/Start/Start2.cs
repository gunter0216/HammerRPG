using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Game;
using App.Game.Contexts;
using App.Game.States.Start;
using UnityEngine;

namespace App.Start
{
    [Scoped(typeof(StartSceneContext))]
    [Stage(typeof(StartInitPhase), 1)]
    public class Start2 : IInitSystem
    {
        public void Init()
        {
            // Debug.LogError("Start2");
        }
    }
}