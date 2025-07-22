using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.GameItem.Runtime.Services;
using App.Common.GameItem.Tests.Mock;
using NUnit.Framework;

namespace App.Common.GameItem.Tests
{
    public class ModulesHolderTests
    {
        private IList<DataReference> m_ModuleRefs;
        
        private IModulesHolder m_ModulesHolder;

        [SetUp]
        public void SetUp()
        {
            var dataContainerController = new MockDataContainerController();
            m_ModuleRefs = new List<DataReference>();
            m_ModulesHolder = new ModulesHolder(dataContainerController, m_ModuleRefs);
        }
        
        [Test]
        public void AddModule_Test1_TrueReturned()
        {
            var moduleData = new Test1ModuleData();
            
            var status = m_ModulesHolder.AddModule(moduleData);
            
            Assert.True(status);
            Assert.AreEqual(1, m_ModuleRefs.Count);
            Assert.AreEqual(moduleData.GetModuleKey(), m_ModuleRefs[0].Key);
            Assert.AreEqual(0, m_ModuleRefs[0].Index);
        }
        
        [Test]
        public void RemoveModule_AddRemoveTest1_TrueReturned()
        {
            var moduleData = new Test1ModuleData();
            
            m_ModulesHolder.AddModule(moduleData);
            var removeStatus = m_ModulesHolder.RemoveModule(moduleData);
            
            Assert.True(removeStatus);
            Assert.AreEqual(0, m_ModuleRefs.Count);
        }
        
        [Test]
        public void HasModule_AddTest1_TrueReturned()
        {
            var moduleData = new Test1ModuleData();
            
            m_ModulesHolder.AddModule(moduleData);
            var isExists = m_ModulesHolder.HasModule<Test1ModuleData>();
            
            Assert.True(isExists);
        }
        
        [Test]
        public void GetModule_AddTest1_TrueReturned()
        {
            var moduleData = new Test1ModuleData();
            
            m_ModulesHolder.AddModule(moduleData);
            var module = m_ModulesHolder.GetModule<Test1ModuleData>();
            
            Assert.True(module.HasValue);
            Assert.AreEqual(moduleData, module.Value);
        }
    }
}