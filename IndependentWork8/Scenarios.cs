using System;
using System.Collections.Generic;
using System.Linq;

namespace IndependentWork8;

//Сценарій 1: Кеш метаданих (Dictionary) 
public class FileMetadata
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[Файл: {FileName}, Розмір: {Size} B, Змінено: {LastModified:g}, Hash: {Hash}]";
    }
}

public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache = new(StringComparer.OrdinalIgnoreCase);

    public void Add(string path, FileMetadata metadata)
    {
        _cache[path] = metadata;
    }

    public FileMetadata Get(string path)
    {
        if (_cache.TryGetValue(path, out var metadata))
        {
            return metadata;
        }
        throw new KeyNotFoundException($"Метадані для шляху '{path}' не знайдено.");
    }

    public bool Remove(string path)
    {
        return _cache.Remove(path);
    }

    public bool Contains(string path)
    {
        return _cache.ContainsKey(path);
    }
}

//Сценарій 2: Унікальні теги (HashSet)
public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags = new();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            _articleTags[articleId] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
        _articleTags[articleId].Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out var tags))
        {
            tags.Remove(tag);
        }
    }

    public HashSet<string> GetAllUniqueTags()
    {
        var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var tags in _articleTags.Values)
        {
            allTags.UnionWith(tags);
        }
        return allTags;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        if (_articleTags.TryGetValue(articleId1, out var tags1) &&
            _articleTags.TryGetValue(articleId2, out var tags2))
        {
            var common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            common.IntersectWith(tags2);
            return common;
        }
        return new HashSet<string>();
    }

    public HashSet<string> GetArticleTags(string articleId)
    {
        return _articleTags.TryGetValue(articleId, out var tags) ? tags : new HashSet<string>();
    }
}

//Сценарій 3: Індекс для пошуку (Dictionary + HashSet) 
public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex = new(StringComparer.OrdinalIgnoreCase);

    public void AddDocument(int docId, string content)
    {
        var words = content.Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (!_invertedIndex.ContainsKey(word))
            {
                _invertedIndex[word] = new HashSet<int>();
            }
            _invertedIndex[word].Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        return _invertedIndex.TryGetValue(word, out var docs) ? docs : new HashSet<int>();
    }

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words == null || words.Length == 0) return new HashSet<int>();

        HashSet<int>? result = null;

        foreach (var word in words)
        {
            var docs = Search(word);
            if (result == null)
            {
                result = new HashSet<int>(docs);
            }
            else
            {
                result.IntersectWith(docs);
            }
        }

        return result ?? new HashSet<int>();
    }
}