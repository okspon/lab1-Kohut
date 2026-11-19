using System;
using System.Collections.Generic;
using Xunit;
using IndependentWork7;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_And_Get_ShouldStoreAndRetrieveItem()
        {
            var cache = new Cache<int, string>(3);
            cache.Add(1, "One");

            var result = cache.Get(1);

            Assert.Equal("One", result);
        }

        [Fact]
        public void Count_ShouldReturnCurrentNumberOfElements()
        {
            var cache = new Cache<string, int>(5);
            Assert.Equal(0, cache.Count);

            cache.Add("a", 1);
            cache.Add("b", 2);

            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void ContainsKey_ShouldReturnTrueForExistingKey_AndFalseForMissing()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("k1", "v1");

            Assert.True(cache.ContainsKey("k1"));
            Assert.False(cache.ContainsKey("k2"));
        }

        [Fact]
        public void Get_NonExistentKey_ShouldThrowKeyNotFoundException()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "Value1");

            Assert.Throws<KeyNotFoundException>(() => cache.Get(99));
        }

        [Fact]
        public void Add_WhenCapacityExceeded_ShouldEvictOldestItem_FIFO()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "First");
            cache.Add(2, "Second");

            // Перевищуємо ємність — ключ 1 має видалитися
            cache.Add(3, "Third");

            Assert.Equal(2, cache.Count);
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Fact]
        public void Add_ExistingKey_ShouldUpdateValueWithoutEvicting()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "OldValue");
            cache.Add(2, "Second");

            // Оновлення існуючого ключа не повинно витісняти інші
            cache.Add(1, "NewValue");

            Assert.Equal(2, cache.Count);
            Assert.Equal("NewValue", cache.Get(1));
            Assert.True(cache.ContainsKey(2));
        }
    }
}