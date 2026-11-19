using System;
using System.Collections.Generic;
using InvertedIndexDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// 1. Демонстрація на 5 базових документах
var baseDocuments = new List<Document>
{
    new Document
    {
        Id = 1,
        Title = "Introduction to C#",
        Content = "C# is a modern object-oriented programming language developed by Microsoft."
    },
    new Document
    {
        Id = 2,
        Title = "Java Basics",
        Content = "Java is a popular programming language known for its portability and performance."
    },
    new Document
    {
        Id = 3,
        Title = "Python Overview",
        Content = "Python is a high-level programming language with simple syntax."
    },
    new Document
    {
        Id = 4,
        Title = "C# Advanced Features",
        Content = "C# supports LINQ, async/await, and many modern programming paradigms."
    },
    new Document
    {
        Id = 5,
        Title = "Comparing Languages",
        Content = "C#, Java, and Python are among the most popular programming languages today."
    }
};

var index = new InvertedIndex();
foreach (var doc in baseDocuments)
{
    index.AddDocument(doc);
}

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words\n");

Console.WriteLine(" Пошук одного слова: 'python' ");
foreach (var doc in index.SearchSingleWord("python"))
    Console.WriteLine($"  {doc}");

Console.WriteLine("\n Пошук ВСІХ слів (AND): 'programming language' ");
foreach (var doc in index.SearchMultipleWords("programming", "language"))
    Console.WriteLine($"  {doc}");

Console.WriteLine("\n Пошук БУДЬ-ЯКОГО слова (OR): 'linq syntax'");
foreach (var doc in index.SearchAnyWord("linq", "syntax"))
    Console.WriteLine($"  {doc}");

// 2. Вимірювання продуктивності (10, 100, 1000, 10000)

Console.WriteLine("ПОРІВНЯННЯ ПРОДУКТИВНОСТІ ПОШУКУ");

int[] sizes = [10, 100, 1000, 10000];
foreach (var size in sizes)
{
    var generatedDocs = PerformanceTest.GenerateDocuments(size);
    PerformanceTest.CompareSearchMethods(generatedDocs, "python");
}

// 3. Інтерактивний режим

Console.WriteLine("Інтерактивний пошук (AND) по 5 базових документах");
while (true)
{
    Console.Write("Enter search query (or 'exit'): ");
    var query = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(query) || query.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    Console.WriteLine("\nSearch Results (AND)");
    var results = index.SearchMultipleWords(words);
    foreach (var doc in results)
    {
        Console.WriteLine(doc);
        Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
    }
    Console.WriteLine($"\nFound {results.Count} documents\n");
}