using System;
using System.Collections.Generic;
using System.Linq;

namespace IndependentWork14_Collections
{
    public class SearchIndex
    {
        private readonly Dictionary<string, HashSet<int>> _invertedIndex = new(StringComparer.OrdinalIgnoreCase);
        private static readonly char[] Separators = { ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t' };

        public void AddDocument(int docId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            var words = content.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (!_invertedIndex.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _invertedIndex[word] = docIds;
                }
                docIds.Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            return _invertedIndex.TryGetValue(word, out var docIds)
                ? new HashSet<int>(docIds)
                : new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words == null || words.Length == 0)
                return new HashSet<int>();

            var result = Search(words[0]);
            for (int i = 1; i < words.Length && result.Count > 0; i++)
            {
                result.IntersectWith(Search(words[i]));
            }
            return result;
        }
    }
}