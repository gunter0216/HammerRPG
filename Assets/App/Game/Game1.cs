using App.Common.HammerDI.Runtime.Attributes;
using App.Game.Contexts;
using UnityEngine;

namespace App.Game
{
    [Scoped(typeof(GameSceneContext))]
    public class Game1 : IInitSystem
    {
        public void Init()
        {
            Debug.LogError("Game1");
        }
    }
}