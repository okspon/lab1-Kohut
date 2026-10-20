using System;
using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public void AddDocument(Document doc)
    {
        _documents[doc.Id] = doc;

        var tokens = TextProcessor.Tokenize(doc.Content);
        var cleanWords = TextProcessor.RemoveStopWords(tokens);

        foreach (var word in cleanWords)
        {
            if (!_index.ContainsKey(word))
            {
                _index[word] = new HashSet<int>();
            }
            _index[word].Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        var normalized = word.ToLowerInvariant();
        if (_index.TryGetValue(normalized, out var docIds))
        {
            return docIds.Select(id => _documents[id]).ToList();
        }
        return new List<Document>();
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        var cleanWords = TextProcessor.RemoveStopWords(words.SelectMany(TextProcessor.Tokenize)).ToList();
        if (!cleanWords.Any()) return new List<Document>();

        HashSet<int>? resultDocIds = null;

        foreach (var word in cleanWords)
        {
            if (_index.TryGetValue(word, out var docIds))
            {
                if (resultDocIds == null)
                {
                    resultDocIds = new HashSet<int>(docIds);
                }
                else
                {
                    resultDocIds.IntersectWith(docIds); // Логічне "AND"
                }
            }
            else
            {
                return new List<Document>(); // Якщо хоча б одного слова немає
            }
        }

        return (resultDocIds ?? new HashSet<int>()).Select(id => _documents[id]).ToList();
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        var cleanWords = TextProcessor.RemoveStopWords(words.SelectMany(TextProcessor.Tokenize)).ToList();
        var resultDocIds = new HashSet<int>();

        foreach (var word in cleanWords)
        {
            if (_index.TryGetValue(word, out var docIds))
            {
                resultDocIds.UnionWith(docIds); // Логічне "OR"
            }
        }

        return resultDocIds.Select(id => _documents[id]).ToList();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;
}