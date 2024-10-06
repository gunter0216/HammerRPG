using System;
using App.Common.HammerDI.External;
using App.Game;
using App.Game.Contexts;
using Leopotam.EcsLite;
using UnityEngine;

namespace App.Menu
{
    public class MenuStartup : MonoBehaviour
    {
        private EcsWorld m_World;
        private EcsSystems m_UpdateSystems;

        void Start()
        {
            var diManager = DiManager.Instance;
            var serviceProvider = diManager.BuildServiceProvider(typeof(MenuSceneContext));

            foreach (IInitSystem initSystem in serviceProvider.GetInterfaces<IInitSystem>())
            {
                initSystem.Init();
            }
            
            foreach (IDisposable disposable in serviceProvider.GetInterfaces<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}