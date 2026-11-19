using System;
using IndependentWork14;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Сценарій 1: FileMetadataCache (Dictionary)");
var fileCache = new FileMetadataCache();
fileCache.Add(@"C:\Docs\report.pdf", new FileMetadata
{
    FileName = "report.pdf",
    Size = 204800,
    LastModified = DateTime.Now,
    Hash = "A1B2C3D4"
});
fileCache.Add(@"C:\Docs\notes.txt", new FileMetadata
{
    FileName = "notes.txt",
    Size = 4096,
    LastModified = DateTime.Now.AddHours(-2),
    Hash = "E5F6G7H8"
});

Console.WriteLine($"Чи є report.pdf (перевірка без урахування регістру): {fileCache.Contains(@"c:\docs\REPORT.PDF")}");
Console.WriteLine($"Отримано: {fileCache.Get(@"C:\Docs\report.pdf")}");
fileCache.Remove(@"C:\Docs\notes.txt");
Console.WriteLine($"Кількість файлів у кеші після видалення: {fileCache.Count}\n");

Console.WriteLine("Сценарій 2: TagManager (HashSet)");
var tagManager = new TagManager();
tagManager.AddTag("post-1", "C#");
tagManager.AddTag("post-1", "DOTNET");
tagManager.AddTag("post-1", "c#"); // Дублікат не додасться
tagManager.AddTag("post-1", "Performance");

tagManager.AddTag("post-2", "dotnet");
tagManager.AddTag("post-2", "Architecture");
tagManager.AddTag("post-2", "Performance");

Console.WriteLine($"Усі унікальні теги (UnionWith): {string.Join(", ", tagManager.GetAllUniqueTags())}");
Console.WriteLine($"Спільні теги post-1 і post-2 (IntersectWith): {string.Join(", ", tagManager.FindCommonTags("post-1", "post-2"))}");
Console.WriteLine($"Унікальні теги лише для post-1 (ExceptWith): {string.Join(", ", tagManager.FindUniqueTagsForFirstArticle("post-1", "post-2"))}");
tagManager.RemoveTag("post-1", "Performance");
Console.WriteLine($"Спільні теги після видалення 'Performance' з post-1: {string.Join(", ", tagManager.FindCommonTags("post-1", "post-2"))}\n");

Console.WriteLine("Сценарій 3: SearchIndex (Dictionary + HashSet)");
var searchIndex = new SearchIndex();
searchIndex.AddDocument(1, "C# and .NET provide fast generic collections like Dictionary and HashSet.");
searchIndex.AddDocument(2, "Dictionary uses a hash table for O(1) key lookup in .NET.");
searchIndex.AddDocument(3, "HashSet stores unique elements and supports set operations.");

Console.WriteLine($"Документи зі словом 'Dictionary': [{string.Join(", ", searchIndex.Search("Dictionary"))}]");
Console.WriteLine($"Документи зі словами 'Dictionary' ТА '.NET': [{string.Join(", ", searchIndex.SearchMultiple("Dictionary", ".NET"))}]");