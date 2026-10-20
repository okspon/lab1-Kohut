using System;
using System.Text;

namespace InvertedIndexDemo;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine(" ВИМІРЮВАННЯ ПРОДУКТИВНОСТІ");
        int[] counts = { 10, 100, 1000, 10000 };
        foreach (var count in counts)
        {
            var generatedDocs = PerformanceTest.GenerateDocuments(count);
            PerformanceTest.CompareSearchMethods(generatedDocs, "programming");
        }

        Console.WriteLine("ІНТЕРАКТИВНИЙ ПОШУК ");
        var index = new InvertedIndex();
        var baseDocs = PerformanceTest.GetBaseDocuments();

        foreach (var doc in baseDocs)
        {
            index.AddDocument(doc);
        }

        Console.WriteLine($"Завантажено документів: {index.DocumentCount}");
        Console.WriteLine($"Унікальних слів у індексі: {index.UniqueWords}\n");

        while (true)
        {
            Console.Write("Enter search query (or 'exit'): ");
            var query = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(query) || query == "exit") break;

            var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("\n--- Search Results (AND) ---");
            var results = index.SearchMultipleWords(words);

            foreach (var doc in results)
            {
                Console.WriteLine(doc);
                string snippet = doc.Content.Length > 100 ? doc.Content.Substring(0, 100) + "..." : doc.Content;
                Console.WriteLine($"  {snippet}");
            }
            Console.WriteLine($"\nЗнайдено документів: {results.Count}\n");
        }
    }
}