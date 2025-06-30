using System.Collections.Generic;
using System.Linq;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Game.Contexts;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Game.Update.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), -100)]
    public class UpdateManager : IInitSystem
    {
        [Inject] private List<IRunSystem> m_RunSystems;

        private List<IRunSystem> m_SortedRunSystems;

        public void Init()
        {
            var systems = new List<OrderedItem<IRunSystem>>(m_RunSystems.Count);
            foreach (var runSystem in m_RunSystems)
            {
                var type = runSystem.GetType();
                var attribute = type.GetAttribute<RunSystemAttribute>();
                if (attribute == null)
                {
                    HLogger.LogError($"{type.Name} implement IRunSystem but not have attribute");
                    continue;
                }
                
                systems.Add(new OrderedItem<IRunSystem>(runSystem, attribute.GetOrder()));
            }
            
            systems.Sort((a, b) => a.Order.CompareTo(b.Order));
            m_SortedRunSystems = systems.Select(x => x.Item).ToList();
        }

        public void Run()
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
    }
}