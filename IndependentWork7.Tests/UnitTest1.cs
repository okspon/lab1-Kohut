using System;
using System.Collections.Generic;
using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests;

public class CacheTests
{
    [Fact]
    public void AddAndGet_ShouldStoreAndReturnCorrectValue()
    {
        var cache = new Cache<string, int>(2);

        cache.Add("item1", 100);

        Assert.Equal(100, cache.Get("item1"));
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void ContainsKey_ShouldReturnTrueForExistingKey()
    {
        var cache = new Cache<int, string>(2);

        cache.Add(1, "One");

        Assert.True(cache.ContainsKey(1));
        Assert.False(cache.ContainsKey(2));
    }

    [Fact]
    public void Get_NonExistingKey_ShouldThrowKeyNotFoundException()
    {
        var cache = new Cache<string, string>(2);

        Action action = () => cache.Get("missing");

        Assert.Throws<KeyNotFoundException>(action);
    }

    [Fact]
    public void FifoEviction_WhenCapacityExceeded_ShouldRemoveOldestElement()
    {
        var cache = new Cache<string, string>(2);

        cache.Add("first", "1");
        cache.Add("second", "2");
        cache.Add("third", "3");

        Assert.Equal(2, cache.Count);
        Assert.False(cache.ContainsKey("first"));
        Assert.True(cache.ContainsKey("second"));
        Assert.True(cache.ContainsKey("third"));
    }
}