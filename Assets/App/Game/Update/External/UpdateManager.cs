using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UniRx;

namespace App.Game.Update.External
{
    public class UpdateManager : IInitSystem, IDisposable
    {
        private readonly List<IRunSystem> m_RunSystems;

        private List<IRunSystem> m_SortedRunSystems;
        
        private CompositeDisposable m_Disposables = new();

        public UpdateManager(List<IRunSystem> runSystems)
        {
            m_RunSystems = runSystems;
        }

        public void Init()
        {
            var systems = new List<OrderedItem<IRunSystem>>(m_RunSystems.Count);
            foreach (var runSystem in m_RunSystems)
            {
                var type = runSystem.GetType();
                var order = UpdateRegistrar.GetOrder(type);
                if (!order.HasValue)
                {
                    HLogger.LogError("Order not found");
                    continue;
                }
                
                systems.Add(new OrderedItem<IRunSystem>(runSystem, order.Value));
            }
            
            systems.Sort((a, b) => a.Order.CompareTo(b.Order));
            m_SortedRunSystems = systems.Select(x => x.Item).ToList();
            
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Run();
                })
                // AddTo(this) ensures the subscription is disposed when the GameObject is destroyed
                .AddTo(m_Disposables); 
        }

        private void Run()
        {
            // Debug.LogError("Run");
            if (m_SortedRunSystems == null)
            {
                return;
            }
            
            foreach (var runSystem in m_SortedRunSystems)
            {
                runSystem.Run();
            }
        }

        public void Dispose()
        {
            m_Disposables?.Dispose();
        }
    }
}