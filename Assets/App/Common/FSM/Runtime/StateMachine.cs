using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.Utility.Runtime.Extensions;
using UnityEngine;

namespace App.Common.FSM.Runtime
{
    // todo несколько фаз нужно перенести в build логику или в синк ран, хз, сортировку по стадиям
    public class StateMachine : IStateMachine
    {
        private readonly List<IState> m_States;
        private readonly Dictionary<Type, List<FSMIItemnfo>> m_Info;
        private readonly Dictionary<int, List<OrderedItem<IInitSystem>>> m_NameToSystems;
        private readonly Dictionary<int, List<OrderedItem<IPostInitSystem>>> m_NameToPostSystems;

        public StateMachine(
            List<IInitSystem> systems, 
            List<IPostInitSystem> postSystems, 
            Dictionary<Type, List<FSMIItemnfo>> info)
        {
            m_Info = info;
            m_States = new List<IState>();
            m_NameToSystems = new Dictionary<int, List<OrderedItem<IInitSystem>>>(systems.Count);
            m_NameToPostSystems = new Dictionary<int, List<OrderedItem<IPostInitSystem>>>(postSystems.Count);

            ParseSystems(m_NameToSystems, systems);
            ParseSystems(m_NameToPostSystems, postSystems);
        }

        private void ParseSystems<T>(Dictionary<int, List<OrderedItem<T>>> dictionary, List<T> systems)
        {
            if (systems.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (var system in systems)
            {
                var systemType = system.GetType();
                if (!m_Info.TryGetValue(systemType, out var infos))
                {
                    Debug.LogError($"not found info for {systemType.Name}");
                    continue;
                }

                foreach (var info in infos)
                {
                    var name = info.Stage;
                    if (!dictionary.TryGetValue(name, out var initSystems))
                    {
                        initSystems = new List<OrderedItem<T>>(1);
                        dictionary.Add(name, initSystems);
                    }
                
                    initSystems.Add(new OrderedItem<T>(system, info.Order));
                }
            }

            SortSystems(dictionary);
        }

        private void SortSystems<T>(Dictionary<int, List<OrderedItem<T>>> dictionary)
        {
            foreach (var systems in dictionary.Values)
            {
                systems.Sort((x, y) => x.Order.CompareTo(y.Order));
            }
        }

        public void AddState(IState state)
        {
            var name = state.GetStage();
            if (!m_NameToSystems.TryGetValue(name, out var systems))
            {
                Debug.LogError($"Systems not found {name}");
                return;
            }
            
            if (!m_NameToPostSystems.TryGetValue(name, out var postSystems))
            {
                postSystems = new List<OrderedItem<IPostInitSystem>>();
            }

            state.SetSystems(
                systems.Select(x => x.Item).ToList(),
                postSystems.Select(x => x.Item).ToList());
            m_States.Add(state);
        }

        public void SyncRun()
        {
            for (int i = 0; i < m_States.Count; ++i)
            {
                m_States[i].SyncRun();
            }
        }
    }
}