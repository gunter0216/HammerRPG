﻿using App.Common.HammerDI.Runtime.Attributes;
using App.Game;
using App.Game.Contexts;
using UnityEngine;

namespace App.Start
{
    [Scoped(typeof(StartSceneContext))]
    public class Start2 : IInitSystem
    {
        public void Init()
        {
            Debug.LogError("Start2");
        }
    }
}