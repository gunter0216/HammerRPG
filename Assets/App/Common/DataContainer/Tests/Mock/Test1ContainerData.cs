using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;

namespace App.Common.DataContainer.Tests.Mock
{
    public class Test1ContainerData : IContainerData
    {
        private List<string> m_Data;

        IList IContainerData.Data => m_Data;
        
        public List<string> Data
        {
            get => m_Data;
            set => m_Data = value;
        }

        public Test1ContainerData()
        {
            m_Data = new List<string>();
        }
        
        public string GetContainerKey()
        {
            return Constants.Test1DataKey;
        }

        public string Name()
        {
            return nameof(Test1ContainerData);
        }
    }
}