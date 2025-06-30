using System;
using System.Collections.Generic;
using App.Common.AssemblyManager.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using App.Common.Data.Runtime;
using App.Common.Data.Runtime.JsonLoader;
using App.Common.Data.Runtime.JsonSaver;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Game.States.Start;

namespace App.Common.Data.External
{
    [Singleton]
    [Stage(typeof(StartInitPhase), -100_000)]
    public class DataManagerProxy : IDataManager, IInitSystem, ISingleton
    {
        [Inject] private readonly IJsonLoader m_Loader;
        [Inject] private readonly IJsonSaver m_Saver;
        
        private DataManager m_DataManager;

        public void OnInjectEnd()
        {
            m_DataManager = new DataManager(m_Loader, m_Saver);
        }
        
        public void Init()
        {
            m_DataManager.Init();
        }

        public void SaveProgress()
        {
            m_DataManager.SaveProgress();
        }

        public Optional<IData> GetData(string name)
        {
            return m_DataManager.GetData(name);
        }

        public void SetDatas(IReadOnlyList<AttributeNode> datasAttributeNodes)
        {
            var datas = new List<IData>(datasAttributeNodes.Count);
            for (int i = 0; i < datasAttributeNodes.Count; ++i)
            {
                var holder = datasAttributeNodes[i].Holder;
                var instance = Activator.CreateInstance(holder) as IData;
                if (instance == null)
                {
                    HLogger.LogError($"data {datasAttributeNodes[i].Holder.Name} contains attribute but no interface");
                    continue;
                }
                
                datas.Add(instance);
            }
            
            m_DataManager.SetDatas(datas);
        }
    }
}