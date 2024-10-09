using System;
using App.Common.HammerDI.External;
using App.Game;
using App.Game.Contexts;
using Leopotam.EcsLite;
using UnityEngine;
using IServiceProvider = App.Common.HammerDI.Runtime.Interfaces.IServiceProvider;

namespace App.Menu
{
    public class MenuStartup : MonoBehaviour
    {
        private EcsWorld m_World;
        private EcsSystems m_UpdateSystems;
        private IServiceProvider m_ServiceProvider;

        private void Start()
        {
            var diManager = DiManager.Instance;
            m_ServiceProvider = diManager.BuildServiceProvider(typeof(MenuSceneContext));

            foreach (IInitSystem initSystem in m_ServiceProvider.GetInterfaces<IInitSystem>())
            {
                initSystem.Init();
            }
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in m_ServiceProvider.GetInterfaces<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}