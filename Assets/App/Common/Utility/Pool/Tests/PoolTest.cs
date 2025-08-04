using System.Collections.Generic;
using App.Common.Utility.Pool.Runtime;
using App.Common.Utility.Pool.Tests.Items;
using App.Common.Utility.Runtime;
using NUnit.Framework;

namespace App.Common.Utility.Pool.Tests
{
    public class PoolTest
    {
        [Test]
        public void GetSuccessfulTest()
        {
            var createdItem = new SimpleItem();
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(createdItem));

            var item = pool.Get();
            var itemHolder = item.Value;

            Assert.True(item.HasValue);
            Assert.NotNull(itemHolder);
            Assert.NotNull(itemHolder.Item);
            Assert.True(itemHolder.IsActive);
            Assert.AreEqual(createdItem, itemHolder.Item);
            Assert.AreEqual(pool.CountActiveItems, 1);
        }
        
        [Test]
        public void GetFailedTest()
        {
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Fail());

            var item = pool.Get();

            Assert.False(item.HasValue);
            Assert.Zero(pool.CountActiveItems);
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
            var items = new List<PoolItemHolder<SimpleItem>>(totalItems);
            for (int i = 0; i < totalItems; ++i)
            {
                var item = pool.Get();
                var itemHolder = item.Value;
                
                items.Add(itemHolder);
                
                Assert.True(item.HasValue);
                Assert.True(itemHolder.IsActive);
                Assert.AreEqual(i, itemHolder.Item.Id);
                Assert.AreEqual(pool.CountActiveItems, i + 1);
            }

            foreach (var itemHolder in items)
            {
                Assert.True(itemHolder.IsActive);
            }

            for (int i = 0; i < totalItems; ++i)
            {
                var itemHolder = items[i];
                pool.Release(itemHolder);
                
                Assert.False(itemHolder.IsActive);
                Assert.AreEqual(totalItems - i - 1, pool.CountActiveItems);
            }
        }
        
        [Test]
        public void ReleaseAllTest()
        {
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(new SimpleItem()));
            
            pool.ReleaseAll();
            Assert.AreEqual(0, pool.CountActiveItems);
            
            const int totalItems = 10;
            var items = new List<PoolItemHolder<SimpleItem>>(totalItems);
            for (int i = 0; i < totalItems; ++i)
            {
                var item = pool.Get();
                items.Add(item.Value);    
            }
            
            pool.ReleaseAll();
            Assert.AreEqual(0, pool.CountActiveItems);

            foreach (var item in items)
            {
                Assert.False(item.IsActive);
            }
        }

        [Test]
        public void ReleaseNotExistsTest()
        {
            var pool = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(new SimpleItem()));
            var poolTemp = new ListPool<SimpleItem>(() => Optional<SimpleItem>.Success(new SimpleItem()));
            var notExistsItem = poolTemp.Get();
            
            Assert.False(pool.Release(notExistsItem.Value));
            Assert.Zero(pool.CountActiveItems);

            Assert.True(pool.Get().HasValue);
            Assert.AreEqual(1, pool.CountActiveItems);
            Assert.False(pool.Release(notExistsItem.Value));
            Assert.AreEqual(1, pool.CountActiveItems);
        }

        [Test]
        public void PoolItemTest()
        {
            var poolItem = new PoolItem();
            var pool = new ListPool<PoolItem>(() => Optional<PoolItem>.Success(poolItem));

            var item = pool.Get();

            Assert.NotNull(poolItem.ReturnInPool);
            Assert.True(pool.CountActiveItems == 1);
            
            poolItem.ReturnInPool.Invoke();
            
            Assert.True(pool.CountActiveItems == 0);
            Assert.False(item.Value.IsActive);
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