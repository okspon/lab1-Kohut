using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InvertedIndexDemo;

public class PerformanceTest
{
    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        var sw = new Stopwatch();

        // 1. Лінійний пошук (з ідентичною токенізацією та очищенням від стоп-слів)
        sw.Start();
        var normalizedWord = searchWord.ToLowerInvariant();
        var linearResults = documents.Where(d =>
            TextProcessor.RemoveStopWords(TextProcessor.Tokenize(d.Content)).Contains(normalizedWord)
        ).ToList();
        sw.Stop();
        long linearTicks = sw.ElapsedTicks;

        // 2. Inverted Index (Індексація + Пошук)
        var index = new InvertedIndex();
        foreach (var doc in documents)
        {
            index.AddDocument(doc);
        }

        sw.Restart();
        var indexResults = index.SearchSingleWord(searchWord);
        sw.Stop();
        long indexTicks = sw.ElapsedTicks;

        Console.WriteLine($"Кількість документів: {documents.Count}");
        Console.WriteLine($"  Linear search:  {linearTicks} ticks");
        Console.WriteLine($"  Inverted Index: {indexTicks} ticks");
        Console.WriteLine($"  Перевірка коректності: Linear = {linearResults.Count}, Index = {indexResults.Count}\n");
    }

    public static List<Document> GenerateDocuments(int count)
    {
        var baseDocs = GetBaseDocuments();
        var list = new List<Document>();
        var random = new Random(42);

        for (int i = 1; i <= count; i++)
        {
            var template = baseDocs[random.Next(baseDocs.Count)];
            list.Add(new Document
            {
                Id = i,
                Title = $"{template.Title} #{i}",
                Content = $"{template.Content} Additional random token_{random.Next(1, 100)}."
            });
        }
        return list;
    }

    public static List<Document> GetBaseDocuments()
    {
        return new List<Document>
        {
            new Document { Id = 1, Title = "Introduction to C#", Content = "C# is a modern object-oriented programming language developed by Microsoft." },
            new Document { Id = 2, Title = "Java Basics", Content = "Java is a popular programming language known for its portability and performance." },
            new Document { Id = 3, Title = "Python Overview", Content = "Python is a high-level programming language with simple syntax." },
            new Document { Id = 4, Title = "C# Advanced Features", Content = "C# supports LINQ, async/await, and many modern programming paradigms." },
            new Document { Id = 5, Title = "Comparing Languages", Content = "C#, Java, and Python are among the most popular programming languages today." }
        };
    }
}