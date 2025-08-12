using System;
using System.Collections.Generic;
using System.Linq;
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
        private static List<Type> m_DataTypes = new List<Type>();
        
        [Inject] private readonly IJsonLoader m_Loader;
        [Inject] private readonly IJsonSaver m_Saver;
        
        private DataManager m_DataManager;

        public void OnInjectEnd()
        {
            m_DataManager = new DataManager(m_Loader, m_Saver, new DataSavePathCreator());
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

        public Optional<T> GetData<T>(string name) where T : IData
        {
            return m_DataManager.GetData<T>(name);
        }

        public static void RegisterDataType<T>() where T : IData
        {
            var type = typeof(T);
            if (!m_DataTypes.Contains(type))
            {
                m_DataTypes.Add(type);
            }
        }
        
        public void SetDatas(IReadOnlyList<AttributeNode> datasAttributeNodes)
        {
            var dataTypes = datasAttributeNodes
                .Select(x => x.Holder)
                .Concat(m_DataTypes)
                .ToArray();
            var datas = new List<IData>(dataTypes.Length);
            for (int i = 0; i < dataTypes.Length; ++i)
            {
                var dataType = dataTypes[i];
                var instance = Activator.CreateInstance(dataType) as IData;
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