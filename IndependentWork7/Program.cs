using System.Text;

namespace IndependentWork7;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Демонстрація роботи Cache<TKey, TValue> (FIFO) ===\n");

        Cache<string, string> cache = new Cache<string, string>(3);

        Console.WriteLine("Додаємо: Key1, Key2, Key3");
        cache.Add("Key1", "Значення 1");
        cache.Add("Key2", "Значення 2");
        cache.Add("Key3", "Значення 3");

        Console.WriteLine($"Поточний Count: {cache.Count}");
        Console.WriteLine($"Отримано Key2: {cache.Get("Key2")}\n");

        Console.WriteLine("Додаємо Key4 (кеш повний, Key1 має видалитися)...");
        cache.Add("Key4", "Значення 4");

        Console.WriteLine($"Чи є Key1 у кеші? -> {cache.ContainsKey("Key1")}");
        Console.WriteLine($"Чи є Key4 у кеші? -> {cache.ContainsKey("Key4")}");
        Console.WriteLine($"Поточний Count: {cache.Count}");
    }
}
