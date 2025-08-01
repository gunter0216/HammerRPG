using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Services
{
    public class ModulesHolder : IModulesHolder
    {
        private readonly IContainersDataManager m_ContainersDataManager;
        private readonly IList<DataReference> m_ModuleRefs;
        
        private IList m_ModuleDatas;

        public ModulesHolder(
            IContainersDataManager containersDataManager, 
            IList<DataReference> moduleRefs)
        {
            m_ContainersDataManager = containersDataManager;
            m_ModuleRefs = moduleRefs;
        }

        public bool Initialize()
        {
            m_ModuleDatas = new List<object>(m_ModuleRefs.Count);
            foreach (var reference in m_ModuleRefs)
            {
                var data = m_ContainersDataManager.GetData(reference);
                if (!data.HasValue)
                {
                    return false;
                }
                
                m_ModuleDatas.Add(data.Value);
            }
            
            return true;
        }

        public bool AddModule(IModuleData data)
        {
            var reference = m_ContainersDataManager.AddData(data.GetModuleKey(), data);
            if (!reference.HasValue)
            {
                return false;
            }
            
            m_ModuleRefs.Add(reference.Value);
            m_ModuleDatas.Add(data);
            return true;
        }

        public bool RemoveModule(IModuleData data)
        {
            var reference = m_ContainersDataManager.RemoveData(data.GetModuleKey(), data);
            if (!reference.HasValue)
            {
                return false;
            }

            int i = 0;
            for (; i < m_ModuleDatas.Count; ++i)
            {
                if (ReferenceEquals(m_ModuleDatas[i], data))
                {
                    break;
                }
            }
            
            m_ModuleRefs.RemoveAt(i);
            m_ModuleDatas.RemoveAt(i);
            return true;
        }

        public Optional<T> GetModule<T>() where T : IModuleData
        {
            if (TryGetModule<T>(out var data))
            {
                return Optional<T>.Success(data);
            }
            
            return Optional<T>.Fail();
        }

        public bool TryGetModule<T>(out T data) where T : IModuleData
        {
            for (int i = 0; i < m_ModuleDatas.Count; ++i)
            {
                if (m_ModuleDatas[i] is T module)
                {
                    data = module;
                    return true;
                }
            }

            data = default;
            
            return false;
        }

        public bool HasModule<T>() where T : IModuleData
        {
            for (int i = 0; i < m_ModuleDatas.Count; ++i)
            {
                if (m_ModuleDatas[i] is T)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}