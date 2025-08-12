using System;
using App.Common.ModuleItem.Tests.Mock;
using Assets.App.Common.ModuleItem.Runtime.Config;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;
using NUnit.Framework;

namespace App.Common.ModuleItem.Tests
{
    public class ModuleItemConfigTests
    {
        [Test]
        public void Id_Item_ItemReturned()
        {
            var id = "Item";
            var config = new ModuleItemConfig(id, 0, Array.Empty<IModuleConfig>());
            
            Assert.AreEqual(id, config.Id);
        }
        
        [Test]
        public void HasTag_Zero_ZeroTrue()
        {
            var config = new ModuleItemConfig(String.Empty, 0, Array.Empty<IModuleConfig>());
            
            Assert.True(config.HasTag(0));
        }
        
        [Test]
        public void HasTag_Zero_OneFalse()
        {
            var config = new ModuleItemConfig(String.Empty, 0, Array.Empty<IModuleConfig>());
            
            Assert.False(config.HasTag(1));
        }
        
        [Test]
        public void HasTag_One_OneTrue()
        {
            var config = new ModuleItemConfig(String.Empty, 1, Array.Empty<IModuleConfig>());
            
            Assert.True(config.HasTag(1));
        }
        
        [Test]
        public void HasTag_Two_OneFalse()
        {
            var config = new ModuleItemConfig(String.Empty, 2, Array.Empty<IModuleConfig>());
            
            Assert.IsFalse(config.HasTag(1));
        }
        
        [Test]
        public void HasTag_Two_TwoTrue()
        {
            var config = new ModuleItemConfig(String.Empty, 2, Array.Empty<IModuleConfig>());
            
            Assert.True(config.HasTag(2));
        }

        [Test]
        public void HasTag_Two_ThreeFalse()
        {
            var config = new ModuleItemConfig(String.Empty, 2, Array.Empty<IModuleConfig>());
            
            Assert.IsFalse(config.HasTag(3));
        }

        [Test]
        public void HasTag_Three_OneTrue()
        {
            var config = new ModuleItemConfig(String.Empty, 3, Array.Empty<IModuleConfig>());
            
            Assert.True(config.HasTag(1));
        }

        [Test]
        public void HasTag_Three_TwoTrue()
        {
            var config = new ModuleItemConfig(String.Empty, 3, Array.Empty<IModuleConfig>());
            
            Assert.True(config.HasTag(2));
        }

        [Test]
        public void HasTag_Three_ThreeTrue()
        {
            var config = new ModuleItemConfig(String.Empty, 3, Array.Empty<IModuleConfig>());
            
            Assert.True(config.HasTag(3));
        }
        
        [Test]
        public void GetModule_ZeroModules_Test1ModuleFalse()
        {
            var config = new ModuleItemConfig(String.Empty, 0, Array.Empty<IModuleConfig>());
            
            var module = config.GetModule<Test1ModuleConfig>();
            Assert.False(module.HasValue);
        }
        
        [Test]
        public void GetModule_Test1Module_Test1ModuleReturned()
        {
            var config = new ModuleItemConfig(String.Empty, 0, new IModuleConfig[]
            {
                new Test1ModuleConfig()
            });
            
            var module = config.GetModule<Test1ModuleConfig>();
            Assert.True(module.HasValue);
        }

        [Test]
        public void TryGetModule_ZeroModules_Test1ModuleFalse()
        {
            var config = new ModuleItemConfig(String.Empty, 0, Array.Empty<IModuleConfig>());
            
            var isExists = config.TryGetModule<Test1ModuleConfig>(out var module);
            Assert.False(isExists);
        }

        [Test]
        public void TryGetModule_Test1Module_Test1ModuleReturned()
        {
            var test1Module = new Test1ModuleConfig();
            var config = new ModuleItemConfig(String.Empty, 0, new IModuleConfig[]
            {
                test1Module
            });
            
            var isExists = config.TryGetModule<Test1ModuleConfig>(out var module);
            Assert.True(isExists);
            Assert.AreEqual(test1Module, module);
        }

        [Test]
        public void HasModule_ZeroModules_Test1ModuleFalse()
        {
            var config = new ModuleItemConfig(String.Empty, 0, Array.Empty<IModuleConfig>());
            
            var isExists = config.HasModule<Test1ModuleConfig>();
            Assert.False(isExists);
        }
        
        [Test]
        public void HasModule_Test1Module_Test1ModuleReturned()
        {
            var config = new ModuleItemConfig(String.Empty, 0, new IModuleConfig[]
            {
                new Test1ModuleConfig()
            });
            
            var isExists = config.HasModule<Test1ModuleConfig>();
            Assert.True(isExists);
        }
    }
}