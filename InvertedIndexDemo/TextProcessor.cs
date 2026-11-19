using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo
{
    public static class TextProcessor
    {
        private static readonly HashSet<string> StopWords = new()
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        /// <summary>
        /// Розбиває текст на слова, видаляє пунктуацію, переводить у нижній регістр.
        /// Зберігає символ '#' (наприклад, для "c#").
        /// </summary>
        public static IEnumerable<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Enumerable.Empty<string>();

            var lowerText = text.ToLowerInvariant();
            var matches = Regex.Matches(lowerText, @"[a-zа-яіїєґ0-9#]+");

            return matches.Select(m => m.Value);
        }

        /// <summary>
        /// Видаляє стоп-слова (a, an, the, is, are, etc.)
        /// </summary>
        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
        {
            return words.Where(w => !StopWords.Contains(w));
        }
    }
}