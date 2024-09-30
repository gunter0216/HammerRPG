using App.Common.HammerDI.Runtime.Attributes;
using App.Game;
using App.Game.Contexts;
using UnityEngine;

namespace App.Start
{
    [Scoped(Context = typeof(StartSceneContext))]
    public class Start1 : IInitSystem
    {
        public void Init()
        {
            Debug.LogError("Start1");
        }
    }
}