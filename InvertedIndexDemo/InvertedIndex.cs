using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo
{
    public class InvertedIndex
    {
        // Слово -> Множина ID документів
        private readonly Dictionary<string, HashSet<int>> _index = new();

        // ID -> Документ (для швидкого отримання)
        private readonly Dictionary<int, Document> _documents = new();

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Keys.Count;

        /// <summary>
        /// Додає документ до індексу
        /// </summary>
        public void AddDocument(Document doc)
        {
            if (doc == null) return;

            _documents[doc.Id] = doc;

            // Токенізуємо заголовок і вміст, прибираємо стоп-слова
            var tokens = TextProcessor.Tokenize($"{doc.Title} {doc.Content}");
            var filteredTokens = TextProcessor.RemoveStopWords(tokens);

            foreach (var word in filteredTokens)
            {
                if (!_index.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _index[word] = docIds;
                }
                docIds.Add(doc.Id);
            }
        }

        /// <summary>
        /// Шукає документи, що містять слово
        /// </summary>
        public List<Document> SearchSingleWord(string word)
        {
            var normalized = TextProcessor.Tokenize(word).FirstOrDefault();
            if (string.IsNullOrEmpty(normalized) || !_index.TryGetValue(normalized, out var docIds))
            {
                return new List<Document>();
            }

            return docIds.Select(id => _documents[id]).ToList();
        }

        /// <summary>
        /// Шукає документи, що містять ВСІ слова (AND)
        /// </summary>
        public List<Document> SearchMultipleWords(params string[] words)
        {
            if (words == null || words.Length == 0)
                return new List<Document>();

            var normalizedWords = words
                .SelectMany(TextProcessor.Tokenize)
                .Distinct()
                .ToList();

            if (normalizedWords.Count == 0)
                return new List<Document>();

            // Якщо хоча б одного слова немає в індексі, перетин буде порожнім
            if (!_index.TryGetValue(normalizedWords[0], out var firstSet))
                return new List<Document>();

            var resultIds = new HashSet<int>(firstSet);

            for (int i = 1; i < normalizedWords.Count; i++)
            {
                if (_index.TryGetValue(normalizedWords[i], out var nextSet))
                {
                    resultIds.IntersectWith(nextSet);
                }
                else
                {
                    return new List<Document>();
                }
            }

            return resultIds.Select(id => _documents[id]).ToList();
        }

        /// <summary>
        /// Шукає документи, що містять БУДЬ-ЯКЕ слово (OR)
        /// </summary>
        public List<Document> SearchAnyWord(params string[] words)
        {
            if (words == null || words.Length == 0)
                return new List<Document>();

            var resultIds = new HashSet<int>();

            foreach (var rawWord in words)
            {
                foreach (var word in TextProcessor.Tokenize(rawWord))
                {
                    if (_index.TryGetValue(word, out var docIds))
                    {
                        resultIds.UnionWith(docIds);
                    }
                }
            }

            return resultIds.Select(id => _documents[id]).ToList();
        }
    }
}