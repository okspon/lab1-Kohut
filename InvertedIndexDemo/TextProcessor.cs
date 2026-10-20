using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvertedIndexDemo;

public static class TextProcessor
{
    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Enumerable.Empty<string>();

        // Переведення в нижній регістр та вилучення слів за допомогою регулярного виразу
        return Regex.Matches(text.ToLowerInvariant(), @"\w+")
                    .Select(m => m.Value);
    }

    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        var stopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        return words.Where(w => !stopWords.Contains(w));
    }
}