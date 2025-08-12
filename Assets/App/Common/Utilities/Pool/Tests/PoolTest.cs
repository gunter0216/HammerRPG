using System.Collections.Generic;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Pool.Tests.Items;
using App.Common.Utilities.Utility.Runtime;
using NUnit.Framework;

namespace App.Common.Utilities.Pool.Tests
{
    public class PoolTest
    {
        [Test]
        public void GetSuccessfulTest()
        {
            var createdItem = new SimpleItem();
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(createdItem));

            var item = pool.Get();

            Assert.True(item.HasValue);
            Assert.NotNull(item);
            Assert.AreEqual(createdItem, item.Value);
        }
        
        [Test]
        public void GetFailedTest()
        {
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Fail());

            var item = pool.Get();

            Assert.False(item.HasValue);
        }
        
        [Test]
        public void GetReleaseTest()
        {
            int index = 0;
            var pool = new ListPool<SimpleItem>(() =>
            {
                var item = new SimpleItem
                {
                    Id = index++
                };
                return Optional<SimpleItem>.Success(item);
            });

            const int totalItems = 10;
            var items = new List<SimpleItem>(totalItems);
            for (int i = 0; i < totalItems; ++i)
            {
                var item = pool.Get();
                
                items.Add(item.Value);
                
                Assert.True(item.HasValue);
                Assert.AreEqual(i, item.Value.Id);
            }

            for (int i = 0; i < totalItems; ++i)
            {
                var itemHolder = items[i];
                pool.Release(itemHolder);
            }
        }
        
        [Test]
        public void ReleaseNotExistsTest()
        {
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(new SimpleItem()));
            var poolTemp = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(new SimpleItem()));
            var notExistsItem = poolTemp.Get();
            
            Assert.False(pool.Release(notExistsItem.Value));

            Assert.True(pool.Get().HasValue);
            Assert.False(pool.Release(notExistsItem.Value));
        }

        [Test]
        public void PoolItemTest()
        {
            var poolItem = new PoolItem();
            var pool = new ListPool<PoolItem>(() => Optional<PoolItem>.Success(poolItem));

            var item = pool.Get();

            Assert.NotNull(poolItem.ReturnInPool);
            
            poolItem.ReturnInPool.Invoke();
        }
        
        [Test]
        public void PoolGetListenerTest()
        {
            bool got = false;
            var itemPoolGetListener = new ItemPoolGetListener(() =>
            {
                got = true;
            });
            
            var pool = new ListPool<ItemPoolGetListener>(() => Optional<ItemPoolGetListener>.Success(itemPoolGetListener));
            
            var item = pool.Get();
            
            Assert.True(got);
        }
        
        [Test]
        public void PoolReleaseListenerTest()
        {
            bool released = false;
            var itemPoolReleaseListener = new ItemPoolReleaseListener(() =>
            {
                released = true;
            });
            
            var pool = new ListPool<ItemPoolReleaseListener>(() => Optional<ItemPoolReleaseListener>.Success(itemPoolReleaseListener));
            
            var item = pool.Get();
            
            Assert.True(!released);
            
            pool.Release(item.Value);
            
            Assert.True(released);
        }
    }
}