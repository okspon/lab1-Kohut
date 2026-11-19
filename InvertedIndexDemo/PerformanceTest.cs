using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InvertedIndexDemo
{
    public static class PerformanceTest
    {
        public static List<Document> GenerateDocuments(int count)
        {
            var baseTemplates = new[]
            {
                ("Introduction to C#", "C# is a modern object-oriented programming language developed by Microsoft."),
                ("Java Basics", "Java is a popular programming language known for its portability and performance."),
                ("Python Overview", "Python is a high-level programming language with simple syntax."),
                ("C# Advanced Features", "C# supports LINQ, async/await, and many modern programming paradigms."),
                ("Comparing Languages", "C#, Java, and Python are among the most popular programming languages today.")
            };

            var extraWords = new[] { "cloud", "database", "algorithm", "network", "security", "compiler", "memory", "thread" };
            var random = new Random(42);
            var list = new List<Document>(count);

            for (int i = 1; i <= count; i++)
            {
                var template = baseTemplates[(i - 1) % baseTemplates.Length];
                var extra = $"{extraWords[random.Next(extraWords.Length)]} {extraWords[random.Next(extraWords.Length)]}";

                list.Add(new Document
                {
                    Id = i,
                    Title = $"{template.Item1} #{i}",
                    Content = $"{template.Item2} Additional topic: {extra}."
                });
            }

            return list;
        }

        public static void CompareSearchMethods(List<Document> documents, string searchWord)
        {
            var normalizedWord = TextProcessor.Tokenize(searchWord).FirstOrDefault() ?? string.Empty;

            // Побудова індексу (окремо засікаємо час індексації)
            var index = new InvertedIndex();
            var buildSw = Stopwatch.StartNew();
            foreach (var doc in documents)
            {
                index.AddDocument(doc);
            }
            buildSw.Stop();

            // Прогрів JIT-компілятора перед вимірюванням
            _ = documents.Where(d => TextProcessor.RemoveStopWords(TextProcessor.Tokenize($"{d.Title} {d.Content}")).Contains(normalizedWord)).ToList();
            _ = index.SearchSingleWord(searchWord);

            // 1. Лінійний пошук (без індексу)
            var sw = Stopwatch.StartNew();
            var linearResults = documents
                .Where(d => TextProcessor.RemoveStopWords(TextProcessor.Tokenize($"{d.Title} {d.Content}"))
                    .Contains(normalizedWord))
                .ToList();
            sw.Stop();
            long linearTicks = sw.ElapsedTicks;
            double linearMs = sw.Elapsed.TotalMilliseconds;

            // 2. Пошук через Inverted Index
            sw.Restart();
            var indexResults = index.SearchSingleWord(searchWord);
            sw.Stop();
            long indexTicks = sw.ElapsedTicks;
            double indexMs = sw.Elapsed.TotalMilliseconds;

            double speedup = indexTicks > 0 ? (double)linearTicks / indexTicks : 0;

            Console.WriteLine($"--- Обсяг: {documents.Count} документів (пошук слова '{searchWord}') ---");
            Console.WriteLine($"Час побудови індексу: {buildSw.Elapsed.TotalMilliseconds:F3} мс");
            Console.WriteLine($"Linear search:        {linearTicks,8} ticks ({linearMs:F4} мс)");
            Console.WriteLine($"Inverted Index:       {indexTicks,8} ticks ({indexMs:F4} мс)");
            Console.WriteLine($"Прискорення пошуку:   ~{speedup:F1}x");
            Console.WriteLine($"Перевірка збігу:      Linear = {linearResults.Count}, Index = {indexResults.Count}\n");
        }
    }
}