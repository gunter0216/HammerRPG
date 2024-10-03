using App.Common.HammerDI.Runtime.Attributes;
using App.Game;
using App.Game.Contexts;
using UnityEngine;

namespace App.Start
{
    [Scoped(Context = typeof(StartSceneContext))]
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