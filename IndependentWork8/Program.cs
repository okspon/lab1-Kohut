using System;
using System.Text;

namespace IndependentWork8;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Сценарій 1: Кеш метаданих (FileMetadataCache)");
        var cache = new FileMetadataCache();
        string path = @"C:\docs\report.pdf";

        cache.Add(path, new FileMetadata
        {
            FileName = "report.pdf",
            Size = 2048576,
            LastModified = DateTime.Now,
            Hash = "A1B2C3D4E5"
        });

        Console.WriteLine($"Чи існує кеш для '{path}'? {cache.Contains(path)}");
        Console.WriteLine($"Отримані метадані: {cache.Get(path)}");

        Console.WriteLine("\n Сценарій 2: Унікальні теги (TagManager)");
        var tagManager = new TagManager();
        tagManager.AddTag("art_1", "C#");
        tagManager.AddTag("art_1", "NET");
        tagManager.AddTag("art_1", "OOP");

        tagManager.AddTag("art_2", "C#");
        tagManager.AddTag("art_2", "LINQ");

        Console.WriteLine($"Теги статті 1: {string.Join(", ", tagManager.GetArticleTags("art_1"))}");
        Console.WriteLine($"Усі унікальні теги: {string.Join(", ", tagManager.GetAllUniqueTags())}");
        Console.WriteLine($"Спільні теги для art_1 та art_2: {string.Join(", ", tagManager.FindCommonTags("art_1", "art_2"))}");

        Console.WriteLine("\n Сценарій 3: Інвертований пошуковий індекс (SearchIndex)");
        var index = new SearchIndex();
        index.AddDocument(1, "C# is a modern object oriented programming language.");
        index.AddDocument(2, "NET platform supports C# programming and LINQ queries.");
        index.AddDocument(3, "Object oriented design principles in C#.");

        Console.WriteLine($"Документи зі словом 'C#': {string.Join(", ", index.Search("C#"))}");
        Console.WriteLine($"Документи зі словом 'oriented': {string.Join(", ", index.Search("oriented"))}");
        Console.WriteLine($"Документи, що містять 'C#' ТА 'oriented': {string.Join(", ", index.SearchMultiple("C#", "oriented"))}");
    }
}