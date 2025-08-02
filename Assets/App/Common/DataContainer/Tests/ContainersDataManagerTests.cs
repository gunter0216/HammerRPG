using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.DataContainer.Runtime.Data;
using App.Common.DataContainer.Runtime.Data.Loader;
using App.Common.DataContainer.Tests.Mock;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace App.Common.DataContainer.Tests
{
    public class ContainersDataManagerTests
    {
        public const string Test1DataKey = Constants.Test1DataKey;
        public const string Test2DataKey = Constants.Test2DataKey;
        
        private ContainersDataManager m_DataManager;
        private Test1ContainerData m_Test1ContainerData;
        private Test2ContainerData m_Test2ContainerData;
        
        [SetUp]
        public void SetUp()
        {
            m_Test1ContainerData = new Test1ContainerData();
            m_Test2ContainerData = new Test2ContainerData();
            var dataLoader = Substitute.For<IContainerDataLoader>();
            var containers = new List<IContainerData>()
            {
                m_Test1ContainerData,
                m_Test2ContainerData
            };
            dataLoader.Load().Returns(Optional<IReadOnlyList<IContainerData>>.Success(containers));
            m_DataManager = new ContainersDataManager(dataLoader, new StubLogger());
            m_DataManager.Initialize();
        }
        
        [Test]
        public void AddData_1Object_0IndexReturned()
        {
            var data = string.Empty;
            
            var reference = m_DataManager.AddData(Test1DataKey, data);
            
            Assert.True(reference.HasValue);
            Assert.AreEqual(Test1DataKey, reference.Value.Key);
            Assert.AreEqual(0, reference.Value.Index);
        }

        [Test]
        public void AddData_2Objects_0And1IndexReturned()
        {
            var data = string.Empty;
            
            var reference1 = m_DataManager.AddData(Test1DataKey, data);
            var reference2 = m_DataManager.AddData(Test1DataKey, data);
            
            Assert.True(reference1.HasValue);
            Assert.True(reference2.HasValue);
            Assert.AreEqual(Test1DataKey, reference1.Value.Key);
            Assert.AreEqual(Test1DataKey, reference2.Value.Key);
            Assert.AreEqual(0, reference1.Value.Index);
            Assert.AreEqual(1, reference2.Value.Index);
        }

        [Test]
        public void GetData_AddObject_SameObjectReturned()
        {
            var data = string.Empty;
            
            var reference = m_DataManager.AddData(Test1DataKey, data);
            var returnData = m_DataManager.GetData(reference.Value);
            
            Assert.True(returnData.HasValue);
            Assert.AreEqual(data, returnData.Value);
        }

        [Test]
        public void GetData_Add2Objects_SameObjectsReturned()
        {
            var data1 = string.Empty;
            var data2 = string.Empty;
            
            var reference1 = m_DataManager.AddData(Test1DataKey, data1);
            var reference2 = m_DataManager.AddData(Test1DataKey, data2);
            var returnData1 = m_DataManager.GetData(reference1.Value);
            var returnData2 = m_DataManager.GetData(reference2.Value);
            
            Assert.True(returnData1.HasValue);
            Assert.True(returnData2.HasValue);
            Assert.AreEqual(data1, returnData1.Value);
            Assert.AreEqual(data2, returnData2.Value);
        }

        [Test]
        public void RemoveData_Add3Objects_1ObjectRemove_0IndexReturned()
        {
            string data1 = string.Empty;
            string data2 = string.Empty;
            string data3 = string.Empty;
            
            m_DataManager.AddData(Test1DataKey, data1);
            m_DataManager.AddData(Test1DataKey, data2);
            m_DataManager.AddData(Test1DataKey, data3);
            var removedDataReference = m_DataManager.RemoveData(Test1DataKey, data1);
            
            Assert.True(removedDataReference.HasValue);
            Assert.AreEqual(0, removedDataReference.Value.Index);
        }

        [Test]
        public void RemoveData_Add3Objects_2ObjectRemove_AddObject_1IndexReturned()
        {
            string data1 = "1";
            string data2 = "2";
            string data3 = "3";
            string data4 = "4";
            
            m_DataManager.AddData(Test1DataKey, data1);
            m_DataManager.AddData(Test1DataKey, data2);
            m_DataManager.AddData(Test1DataKey, data3);
            m_DataManager.RemoveData(Test1DataKey, data2);
            var reference = m_DataManager.AddData(Test1DataKey, data4);
            
            Assert.True(reference.HasValue);
            Assert.AreEqual(1, reference.Value.Index);
        }
        
        [Test]
        public void AddData_3ObjectsInOtherContainers_CorrectContainersSize()
        {
            string data1 = string.Empty;
            string data2 = string.Empty;
            string data3 = string.Empty;
            
            m_DataManager.AddData(Test1DataKey, data1);
            m_DataManager.AddData(Test1DataKey, data2);
            
            m_DataManager.AddData(Test2DataKey, data3);
            
            Assert.AreEqual(2, m_Test1ContainerData.Data.Count);
            Assert.AreEqual(1, m_Test2ContainerData.Data.Count);
        }
    }
}