using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game;
using Unity.VisualScripting;

namespace App.Common.FSM.Runtime
{
    public class StateMachine : IStateMachine
    {
        private readonly List<IStage> m_States;
        private readonly Dictionary<string, List<Tuple<int, IInitSystem>>> m_NameToSystems;

        public StateMachine(List<IInitSystem> systems)
        {
            m_States = new List<IStage>();
            m_NameToSystems = new Dictionary<string, List<Tuple<int, IInitSystem>>>(systems.Count);
            
            foreach (var system in systems)
            {
                var type = system.GetType();
                var stage = type.GetAttribute<Stage>(false);
                if (stage == null)
                {
                    HLogger.LogError($"Not found attribute {type.Name}");
                    continue;
                }

                var name = stage.GetName();
                if (!m_NameToSystems.TryGetValue(name, out var initSystems))
                {
                    initSystems = new List<Tuple<int, IInitSystem>>(1);
                    m_NameToSystems.Add(name, initSystems);
                }
                
                initSystems.Add(new Tuple<int, IInitSystem>(stage.GetOrder(), system));
            }

            SortSystems();
        }

        private void SortSystems()
        {
            foreach (var systems in m_NameToSystems.Values)
            {
                systems.Sort((x, y) => x.Item1.CompareTo(y.Item1));
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

            stage.SetSystems(systems.Select(x => x.Item2).ToList());
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