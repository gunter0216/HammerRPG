using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Tests.Mock
{
    public class MockDataContainerController : IContainersDataManager
    {
        private readonly List<object> m_Objects = new List<object>();
        
        public Optional<DataReference> AddData(string key, object data)
        {
            var index = m_Objects.FindIndex(x => x == null);
            if (index >= 0)
            {
                m_Objects[index] = data;
                return Optional<DataReference>.Success(new DataReference(key, index));
            }
            
            m_Objects.Add(data);
            return Optional<DataReference>.Success(new DataReference(key, m_Objects.Count - 1));
        }

        public Optional<DataReference> RemoveData(string key, object data)
        {
            var index = m_Objects.FindIndex(x => x == data);
            if (index >= 0)
            {
                m_Objects[index] = null;
                return Optional<DataReference>.Success(new DataReference(key, index));
            }
            
            return Optional<DataReference>.Fail();
        }

        public Optional<object> GetData(IDataReference dataReference)
        {
            if (dataReference.Index < 0 || dataReference.Index >= m_Objects.Count)
            {
                return Optional<object>.Fail();
            }
            
            return Optional<object>.Success(m_Objects[dataReference.Index]);
        }

        public Optional<T> GetData<T>(IDataReference dataReference) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}