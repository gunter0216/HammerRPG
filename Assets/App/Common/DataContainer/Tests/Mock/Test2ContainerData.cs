using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;

namespace App.Common.DataContainer.Tests.Mock
{
    public class Test2ContainerData : IContainerData
    {
        private List<string> m_Data;

        IList IContainerData.Data => m_Data;
        
        public List<string> Data
        {
            get => m_Data;
            set => m_Data = value;
        }

        public Test2ContainerData()
        {
            m_Data = new List<string>();
        }
        
        public string GetContainerKey()
        {
            return Constants.Test2DataKey;
        }

        public string Name()
        {
            return nameof(Test2ContainerData);
        }
    }
}