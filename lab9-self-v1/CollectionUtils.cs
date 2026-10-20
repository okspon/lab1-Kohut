using System;
using System.Collections.Generic;

namespace lab9_self_v1;

public static class CollectionUtils
{
    // Метод 1: Map (аналог Select)
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        var result = new List<TResult>();
        foreach (var item in collection)
        {
            result.Add(mapper(item));
        }
        return result;
    }

    // Метод 2: MaxBy
    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        T maxItem = default!;
        TKey maxKey = default!;
        bool first = true;

        foreach (var item in collection)
        {
            TKey key = keySelector(item);
            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
        {
            throw new InvalidOperationException("Колекція порожня.");
        }

        return maxItem;
    }

    // Метод 3: Filter (аналог Where)
    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    // Метод 4: ForEach
    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}