using System;
using IndependentWork14_Collections;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// =========================================================================
// СЦЕНАРІЙ 1: Кеш метаданих файлів (Dictionary<string, FileMetadata>)
// =========================================================================
Console.WriteLine("=== Сценарій 1: Кеш метаданих файлів (Dictionary) ===");
var fileCache = new FileMetadataCache();

fileCache.AddOrUpdate(@"C:\Projects\App\Program.cs", new FileMetadata
{
    FileName = "Program.cs",
    Size = 2048,
    LastModified = DateTime.Now.AddMinutes(-15),
    Hash = "A1B2C3D4"
});

fileCache.AddOrUpdate(@"C:\Projects\App\appsettings.json", new FileMetadata
{
    FileName = "appsettings.json",
    Size = 512,
    LastModified = DateTime.Now.AddHours(-2),
    Hash = "E5F6G7H8"
});

// Перевірка регістронезалежного пошуку за ключем (O(1))
string queryPath = @"c:\projects\app\PROGRAM.CS";
Console.WriteLine($"Наявність '{queryPath}': {fileCache.Contains(queryPath)}");
Console.WriteLine($"Отримано з кешу: {fileCache.Get(queryPath)}");

fileCache.Remove(@"C:\Projects\App\appsettings.json");
Console.WriteLine($"Кількість записів після видалення: {fileCache.Count}\n");

// =========================================================================
// СЦЕНАРІЙ 2: Система унікальних тегів (Dictionary + HashSet)
// =========================================================================
Console.WriteLine("=== Сценарій 2: Унікальні теги для статей (HashSet) ===");
var tagManager = new TagManager();

tagManager.AddTag("post-1", "C#");
tagManager.AddTag("post-1", "c#"); // Дублікат у іншому регістрі — не додасться
tagManager.AddTag("post-1", ".NET");
tagManager.AddTag("post-1", "LINQ");

tagManager.AddTag("post-2", ".NET");
tagManager.AddTag("post-2", "Polly");
tagManager.AddTag("post-2", "C#");

Console.WriteLine($"Усі унікальні теги (UnionWith): {string.Join(", ", tagManager.GetAllUniqueTags())}");
Console.WriteLine($"Спільні теги post-1 і post-2 (IntersectWith): {string.Join(", ", tagManager.FindCommonTags("post-1", "post-2"))}");
Console.WriteLine($"Теги лише у post-1 (ExceptWith): {string.Join(", ", tagManager.FindUniqueTagsForFirst("post-1", "post-2"))}");

tagManager.RemoveTag("post-1", "LINQ");
Console.WriteLine($"Теги post-1 після видалення 'LINQ': {string.Join(", ", tagManager.GetTagsForArticle("post-1"))}\n");

// =========================================================================
// СЦЕНАРІЙ 3: Інвертований пошуковий індекс (Dictionary<string, HashSet<int>>)
// =========================================================================
Console.WriteLine("=== Сценарій 3: Інвертований пошуковий індекс ===");
var searchIndex = new SearchIndex();

searchIndex.AddDocument(1, "C# та .NET дозволяють швидко будувати веб-застосунки.");
searchIndex.AddDocument(2, "Колекції Dictionary та HashSet у .NET забезпечують пошук за O(1).");
searchIndex.AddDocument(3, "C# підтримує LINQ для роботи з колекціями даних.");

Console.WriteLine($"Документи зі словом '.NET': [{string.Join(", ", searchIndex.Search(".NET"))}]");
Console.WriteLine($"Документи зі словами 'C#' І '.NET': [{string.Join(", ", searchIndex.SearchMultiple("C#", ".NET"))}]");
