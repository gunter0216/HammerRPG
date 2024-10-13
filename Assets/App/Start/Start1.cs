using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Game;
using App.Game.Contexts;
using App.Game.States.Start;
using UnityEngine;

namespace App.Start
{
    [Scoped(typeof(StartSceneContext))]
    [Stage(typeof(StartInitPhase), 0)]
    public class Start1 : IInitSystem
    {
        [Inject] private MonoScoped1 m_MonoScoped1;
        
        public void Init()
        {
            Debug.LogError("Start1");
            m_MonoScoped1.GetValue();
        }
    }
}