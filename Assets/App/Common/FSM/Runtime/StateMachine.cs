using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Common.Utility.Runtime.Extensions;

namespace App.Common.FSM.Runtime
{
    // todo несколько фаз нужно перенести в build логику или в синк ран, хз, сортировку по стадиям
    public class StateMachine : IStateMachine
    {
        private readonly List<IStage> m_States;
        private readonly Dictionary<string, List<OrderedItem<IInitSystem>>> m_NameToSystems;
        private readonly Dictionary<string, List<OrderedItem<IPostInitSystem>>> m_NameToPostSystems;

        public StateMachine(List<IInitSystem> systems, List<IPostInitSystem> postSystems)
        {
            m_States = new List<IStage>();
            m_NameToSystems = new Dictionary<string, List<OrderedItem<IInitSystem>>>(systems.Count);
            m_NameToPostSystems = new Dictionary<string, List<OrderedItem<IPostInitSystem>>>(postSystems.Count);

            ParseSystems(m_NameToSystems, systems);
            ParseSystems(m_NameToPostSystems, postSystems);
        }

        private void ParseSystems<T>(Dictionary<string, List<OrderedItem<T>>> dictionary, List<T> systems)
        {
            if (systems.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (var system in systems)
            {
                var type = system.GetType();
                var stage = type.GetCustomAttribute<Stage>(false);
                if (stage == null)
                {
                    HLogger.LogError($"Not found attribute {type.Name}");
                    continue;
                }

                var name = stage.GetName();
                if (!dictionary.TryGetValue(name, out var initSystems))
                {
                    initSystems = new List<OrderedItem<T>>(1);
                    dictionary.Add(name, initSystems);
                }
                
                initSystems.Add(new OrderedItem<T>(system, stage.GetOrder()));
            }

            SortSystems(dictionary);
        }

        private void SortSystems<T>(Dictionary<string, List<OrderedItem<T>>> dictionary)
        {
            foreach (var systems in dictionary.Values)
            {
                systems.Sort((x, y) => x.Order.CompareTo(y.Order));
            }
        }

        public void AddState(IStage stage)
        {
            var name = stage.GetName();
            if (!m_NameToSystems.TryGetValue(name, out var systems))
            {
                HLogger.LogError($"Systems not found {name}");
                return;
            }
            
            if (!m_NameToPostSystems.TryGetValue(name, out var postSystems))
            {
                postSystems = new List<OrderedItem<IPostInitSystem>>();
            }

            stage.SetSystems(
                systems.Select(x => x.Item).ToList(),
                postSystems.Select(x => x.Item).ToList());
            m_States.Add(stage);
        }

        public void SyncRun()
        {
            for (int i = 0; i < m_States.Count; ++i)
            {
                m_States[i].SyncRun();
            }
        }

        // public IEnumerator Run()
        // {
        //     for (int i = 0; i < m_States.Count; ++i)
        //     {
        //         m_States[i].Run();
        //     }
        // }
    }
}