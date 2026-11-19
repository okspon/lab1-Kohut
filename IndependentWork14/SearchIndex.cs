using System;
using System.Collections.Generic;
using System.Linq;

namespace IndependentWork14
{
    public class SearchIndex
    {
        // Слово -> Множина ID документів, де воно зустрічається
        private readonly Dictionary<string, HashSet<int>> _invertedIndex =
            new(StringComparer.OrdinalIgnoreCase);

        public void AddDocument(int docId, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            char[] separators = [' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t'];
            var words = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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
            if (_invertedIndex.TryGetValue(word, out var docIds))
            {
                return new HashSet<int>(docIds);
            }
            return new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words == null || words.Length == 0)
                return new HashSet<int>();

            // Беремо документи для першого слова
            var result = Search(words[0]);

            // Перетинаємо з множинами для всіх інших слів
            for (int i = 1; i < words.Length && result.Count > 0; i++)
            {
                var nextDocs = Search(words[i]);
                result.IntersectWith(nextDocs);
            }

            return result;
        }
    }
}